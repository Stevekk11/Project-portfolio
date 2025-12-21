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
    stanice_id   int           not null
        references dbo.stanice
            on delete cascade,
    typ          nvarchar(50)  not null,
    barva        nvarchar(50),
    vlastnik     nvarchar(100) not null,
    spravce      nvarchar(100) not null,
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
        check ([rozchod_kolej] > 650)
)
go

