# -*- mode: python ; coding: utf-8 -*-

from pathlib import Path

from PyInstaller.utils.hooks import collect_data_files, collect_submodules, copy_metadata

project_dir = Path.cwd().resolve()
model_datas = [(str(path), ".") for path in project_dir.glob("*.pt")]

ultralytics_datas = collect_data_files("ultralytics")
pillow_datas = collect_data_files("PIL")
metadata_datas = copy_metadata("ultralytics")

hiddenimports = [
    "cv2",
    *collect_submodules("ultralytics"),
    *collect_submodules("PIL"),
]

block_cipher = None


a = Analysis(
    ["app.py"],
    pathex=[str(project_dir)],
    binaries=[],
    datas=model_datas + ultralytics_datas + pillow_datas + metadata_datas,
    hiddenimports=hiddenimports,
    hookspath=[],
    hooksconfig={},
    runtime_hooks=[],
    excludes=[],
    win_no_prefer_redirects=False,
    win_private_assemblies=False,
    cipher=block_cipher,
    noarchive=False,
)
pyz = PYZ(a.pure, a.zipped_data, cipher=block_cipher)

exe = EXE(
    pyz,
    a.scripts,
    [],
    exclude_binaries=True,
    name="YOLODetectionStudio",
    debug=False,
    bootloader_ignore_signals=False,
    strip=False,
    upx=True,
    console=False,
    disable_windowed_traceback=False,
)
coll = COLLECT(
    exe,
    a.binaries,
    a.zipfiles,
    a.datas,
    strip=False,
    upx=True,
    upx_exclude=[],
    name="YOLODetectionStudio",
)

