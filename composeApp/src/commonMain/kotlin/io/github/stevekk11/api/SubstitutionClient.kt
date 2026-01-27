package io.github.stevekk11.api

class SubstitutionClient
{
    private var baseUrl = ""
    private val classSymbol = ""

    fun setEndpointUrlUrl(url: String)
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

}