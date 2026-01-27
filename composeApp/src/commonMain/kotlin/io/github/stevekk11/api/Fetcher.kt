package io.github.stevekk11.api

import io.ktor.client.HttpClient
import io.ktor.client.engine.cio.CIO
import io.ktor.client.request.get
import io.ktor.client.statement.HttpResponse
import io.ktor.client.statement.bodyAsText

class Fetcher
{
    companion object
    {
        suspend fun fetchJsonFromApi(endpoint: String): String
        {
            val client = HttpClient(CIO)
            try
            {
                val response: HttpResponse = client.get(endpoint)
                return response.bodyAsText()
            } catch (e: Exception){
                throw e
            } finally
            {
                client.close()
            }
        }
    }
}