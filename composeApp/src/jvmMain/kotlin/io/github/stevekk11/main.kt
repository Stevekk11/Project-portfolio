package io.github.stevekk11

import androidx.compose.ui.window.Window
import androidx.compose.ui.window.application

fun main() = application {
    Window(
        onCloseRequest = ::exitApplication,
        title = "JecnaSupl",
    ) {
        App()
    }
}