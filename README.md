# JecnaSupl 📅

Knihovna pro snadné získávání dat o suplování ze systému SPŠE Ječná. Už žádné složité parsování webu, stačí pár řádků kódu a máš vše, co potřebuješ.

## Co to umí?
-  Získat suplování pro konkrétní třídu.
- Přehled chybějících učitelů.
- Informace o tom, kdy bylo suplování naposledy aktualizováno.
- Jednoduché použití díky Kotlin Multiplatform.

## Jak na to?

Použití je fakt jednoduché. Takhle získáš suplování pro svou třídu:

```kotlin
val client = SubstitutionClient()

// 1. Nastavíš, odkud se mají data brát
client.setEndpointUrl("https://jecnarozvrh.jzitnik.dev/versioned/v1")

// 2. Vybereš svou třídu (třeba C4c, A2b, E3...)
client.setClassSymbol("C4c")

// 3. A je to! Teď už jen data stáhneš
val substitutions = client.getSubstitutions()

substitutions?.forEach { lesson ->
    println("Hodina: ${lesson.hour}. | Předmět: ${lesson.subject} | Učitel: ${lesson.substitutingTeacher}")
}
```

### Chceš vědět, kteří učitelé chybí?
Můžeš získat seznam všech absencí, nebo jen pro konkrétní den:

```kotlin
// Všechny nahlášené absence
val absences = client.getTeacherAbsences()

// Absence pro konkrétní datum (vyžaduje java.time.LocalDate)
val date = LocalDate.now()
val dailyAbsence = client.getTeacherAbsences(date)
```

### Podrobné suplování po dnech
Pokud potřebuješ data rozdělená přesně podle dnů (např. pro zobrazení v rozvrhu):

```kotlin
val dailySubs = client.getDailySubstitutions()

dailySubs?.forEach { day ->
    println("Den: ${day.date}")
    day.classSubs.values.flatten().forEach { sub ->
        println("  - ${sub.subject} (${sub.hour}. hodina)")
    }
}
```

### Kompletní data (Všechno v jednom)
Chceš úplně všechna data, která jsou k dispozici (všechny třídy, všechny absence, status)?

```kotlin
val complete = client.getCompleteSchedule()
println("Poslední aktualizace: ${complete?.status?.lastUpdated}")
// complete.dailySchedules obsahuje data pro všechny třídy v dané dny
```

### Je suplování aktuální?
Můžeš si ověřit, kdy se data naposledy změnila a jestli je server dostupný:

```kotlin
val status = client.getSubstitutionsStatus()
println("Naposledy aktualizováno: ${status.lastUpdated}")

if (status.isOffline == true) {
    println("Pozor, data nejsou dostupná!")
}
```

### Surová data (pro fajnšmekry)
Pokud si chceš JSON zpracovat po svém, můžeš si ho nechat prostě poslat:

```kotlin
val rawJson = client.getRawSubstitutionData()
println(rawJson)
```

## Požadavky
- Kotlin (Multiplatform)
- Trocha trpělivosti při čekání na to, až se v suplování objeví tvoje hodina.

---
Vytvořeno pro studenty (a možná i učitele) z Ječné.