package io.github.stevekk11.dtos

import kotlinx.serialization.Serializable

@Serializable
data class SubstitutionStatus(
    val lastUpdated: String,
    val currentUpdateSchedule: Int,
    val message: String? = null
)
