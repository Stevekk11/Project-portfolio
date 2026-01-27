package io.github.stevekk11.api

import io.github.stevekk11.dtos.LabeledTeacherAbsences
import io.github.stevekk11.dtos.SubstitutedLesson

class SubstitutionClient
{
    private var baseUrl = ""
    private val classSymbol = ""

    fun setEndpointUrl(url: String)
    {
        if (!url.contains("jecnarozvrh"))
        {
            throw IllegalArgumentException("Invalid substitution endpoint URL. Must contain 'jecnarozvrh'")
        }
        baseUrl = url
    }

    fun setClassSymbol(symbol: String)
    {
        if (symbol.isBlank() || symbol.length < 2)
        {
            throw IllegalArgumentException("Invalid class symbol. Examples of valid symbols: 'C2b, A4, E2, C3a'")
        }
    }

    suspend fun getTeacherAbsences(): List<LabeledTeacherAbsences> {
        val json = Fetcher.fetchJsonFromApi(baseUrl)
        //return SubstitutionParser.parseTeacherAbsences(substitutions)
        return emptyList()
    }

    suspend fun getRawSubstitutionData(): String {
        return Fetcher.fetchJsonFromApi(baseUrl)
    }

    suspend fun getSubstitutions() : List<SubstitutedLesson>{
        val json = Fetcher.fetchJsonFromApi(baseUrl)
        //return SubstitutionParser.parseSubstitutions(json, classSymbol)
        return emptyList()
    }
}