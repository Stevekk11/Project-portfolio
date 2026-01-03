-- this script creates the database schema for a public transportation system, made for MS SQL Server.
create database doprava;
go
use doprava
go

create table dbo.linky
(
    id_linky    int identity
        primary key,
    cislo_linky int not null
        constraint UQ_cislo_linky
            unique,
    nazev_linky nvarchar(50) default ''
)
go

create table dbo.stanice
(
    id_stanice    int identity
        primary key,
    ma_lavicku    bit default 1 not null,
    ma_kos        bit default 1 not null,
    ma_pristresek bit default 1,
    ma_infopanel  bit default 0 not null,
    na_znameni    bit default 0 not null,
    bezbarierova  bit default 0 not null,
    typ_stanice   nvarchar(10)  not null
        check ([typ_stanice] = 'bus' OR [typ_stanice] = 'metro' OR [typ_stanice] = 'tram' OR [typ_stanice] = 'vlak'),
    nazev         nvarchar(50)  not null
        constraint UQ_nazev_stanice
            unique
        constraint CHK_nazev_length
            check (len([nazev]) > 0)
)
go

create table dbo.metro_stanice
(
    id               int identity
        primary key,
    stanice_id       int                                    not null
        references dbo.stanice
            on delete cascade,
    hloubka_pod_zemi float                                  not null,
    cetnost_uklidu   nvarchar(50),
    ma_wc            bit      default 0                     not null,
    dat_posl_uklid   datetime default '2025-01-01 10:00:00' not null
        check (datepart(year, [dat_posl_uklid]) >= 2025)
)
go

create table dbo.pristresek
(
    id_prist     int identity
        primary key,
    stanice_id   int                                    not null
        references dbo.stanice
            on delete cascade,
    typ          nvarchar(50)                           not null,
    barva        nvarchar(50),
    vlastnik     nvarchar(100)                          not null,
    spravce      nvarchar(100)                          not null,
    datum_vyroby datetime default '2025-01-01 10:00:00' not null
)
go

create table dbo.stanice_linka
(
    id_sl      int identity
        primary key,
    stanice_id int not null
        references dbo.stanice
            on delete cascade,
    linka_id   int not null
        references dbo.linky
            on delete cascade
)
go

create table dbo.vlak_stanice
(
    id_vlak         int identity
        primary key,
    stanice_id      int
        references dbo.stanice
            on delete cascade,
    pocet_nast      int           not null
        check ([pocet_nast] > 0),
    elektrifikovana bit default 0 not null,
    soustava        nvarchar(50),
    rozchod_kolej   int
        check ([rozchod_kolej] > 650),

)
go

CREATE view dbo.v_stanice_s_linkami as
SELECT s.id_stanice,
       s.nazev                                  AS nazev_stanice,
       s.typ_stanice,
       COALESCE(COUNT(DISTINCT sl.linka_id), 0) AS pocet_linek,
       STRING_AGG(l.cislo_linky, ', ')          AS cisla_linek,
       STRING_AGG(l.nazev_linky, ', ')          AS nazvy_linek,
       s.ma_lavicku,
       s.ma_kos,
       s.ma_pristresek,
       s.ma_infopanel,
       s.na_znameni,
       s.bezbarierova,
       (CASE
            WHEN s.ma_lavicku = 1 AND s.ma_kos = 1 AND s.ma_pristresek = 1 AND s.ma_infopanel = 1
                THEN 'Plně vybavena'
            WHEN s.ma_lavicku = 1 AND s.ma_kos = 1 AND s.ma_pristresek = 1 THEN 'Standardně vybavena'
            ELSE 'Částečně vybavena' END)       AS uroven_vybaveni
FROM dbo.stanice s
         LEFT JOIN dbo.stanice_linka sl ON s.id_stanice = sl.stanice_id
         LEFT JOIN dbo.linky l ON sl.linka_id = l.id_linky
GROUP BY s.id_stanice, s.nazev, s.typ_stanice, s.ma_lavicku, s.ma_kos, s.ma_pristresek, s.ma_infopanel,
         s.na_znameni, s.bezbarierova
go

create view dbo.v_linky_s_pokrytim
as
select l.id_linky,
       l.cislo_linky,
       l.nazev_linky,
       coalesce(count(distinct sl.stanice_id), 0)                                                 as pocet_stanic,
       coalesce(sum(case when s.typ_stanice = 'bus' then 1 else 0 end), 0)                        as pocet_autobusovych,
       coalesce(sum(case when s.typ_stanice = 'metro' then 1 else 0 end), 0)                      as pocet_metrovych,
       coalesce(sum(case when s.typ_stanice = 'tram' then 1 else 0 end), 0)                       as pocet_tramvajovych,
       coalesce(sum(case when s.typ_stanice = 'vlak' then 1 else 0 end), 0)                       as pocet_vlakovych,
       coalesce(min(case when s.typ_stanice = 'metro' then ms.hloubka_pod_zemi else null end), 0) as min_hloubka_metra,
       coalesce(max(case when s.typ_stanice = 'metro' then ms.hloubka_pod_zemi else null end), 0) as max_hloubka_metra
from dbo.linky l
         left join dbo.stanice_linka sl on l.id_linky = sl.linka_id
         left join dbo.stanice s on sl.stanice_id = s.id_stanice
         left join dbo.metro_stanice ms on s.id_stanice = ms.stanice_id
group by l.id_linky, l.cislo_linky, l.nazev_linky

CREATE TRIGGER TR_metro_station_type
    ON dbo.metro_stanice
    AFTER INSERT, UPDATE AS
BEGIN
    IF EXISTS (SELECT 1
               FROM inserted i
                        JOIN dbo.stanice s ON s.id_stanice = i.stanice_id
               WHERE s.typ_stanice <> 'metro')
        BEGIN
            RAISERROR ('Stanice must be of type metro.', 16, 1); ROLLBACK;
        END
END;

CREATE TRIGGER TR_train_station_type
        ON dbo.vlak_stanice
        AFTER INSERT, UPDATE AS
BEGIN
        IF EXISTS (SELECT 1
                   FROM inserted i
                            JOIN dbo.stanice s ON s.id_stanice = i.stanice_id
                   WHERE s.typ_stanice <> 'vlak')
            BEGIN
                RAISERROR ('Stanice must be of type vlak.', 16, 1); ROLLBACK;
            END
    END;

