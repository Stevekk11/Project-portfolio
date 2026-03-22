# YOLO Detekční aplikace

Desktop GUI aplikace v Pythonu pro pohodlné spouštění YOLO detekce nad obrázky i videem.

## Co umí

- výběr libovolného YOLO `.pt` modelu
- detekce nad jedním obrázkem s okamžitým preview originálu a výsledku
- detekce nad videem na pozadí bez zamrznutí UI
- progress bar a textové shrnutí výsledků
- export anotovaných výstupů a JSON metadat
- otevření výsledného souboru nebo výstupní složky jedním kliknutím

## Soubory

- `app.py` – hlavní GUI aplikace
- `inference_service.py` – backend pro image/video inference a ukládání výsledků

## Instalace

Doporučený postup ve virtuálním prostředí:

```powershell
python -m venv .venv
.\.venv\Scripts\Activate.ps1
pip install -r requirements.txt
```

## Spuštění GUI

```powershell
python app.py
```

## Testování

Aplikace obsahuje unit testy pro ověření základní funkcionality pomocných skriptů. Testy můžete spustit pomocí modulu `unittest`:

```powershell
python -m unittest discover tests
```

## Build přenositelného `.exe`

Pro Windows je připravený build přes `PyInstaller`.

```powershell
Set-ExecutionPolicy -Scope Process Bypass
.\build.ps1
```

Výstup najdeš zde:

- portable složka: `dist\YOLODetectionStudio\`
- hlavní spustitelný soubor: `dist\YOLODetectionStudio\YOLODetectionStudio.exe`

Poznámka: build je záměrně ve variantě **one-dir**, protože je spolehlivější pro `torch`, `opencv` a větší YOLO modely než single-file build. Pro přenesení na jiný počítač kopíruj celou složku `dist\YOLODetectionStudio\`.


## Jak aplikaci používat

1. **Výběr modelu:** V horní části vyberte YOLO model (soubor `.pt`). Pokud máte vlastní model, můžete ho přidat pomocí tlačítka "Procházet...".
2. **Nastavení parametrů:**
   - **Confidence threshold:** Určuje minimální jistotu modelu pro uznání detekce.
   - **IOU threshold:** Ovlivňuje překryv boxů (při více detekcích stejného objektu).
3. **Detekce v obrázku:**
   - Přepněte na záložku **Obrázek**.
   - Vyberte vstupní soubor a klikněte na **Detekovat**.
   - Po dokončení uvidíte srovnání originálu a výsledku přímo v aplikaci.
4. **Detekce ve videu:**
   - Přepněte na záložku **Video**.
   - Vyberte video soubor a klikněte na **Spustit zpracování**.
   - Průběh můžete sledovat na progress baru.
5. **Prohlížení výsledků:**
   - Výstupy se ukládají do složky `gui_outputs/` (lze změnit v nastavení).
   - Tlačítkem "Otevřít výstup" nebo "Otevřít složku" se rychle dostanete k výsledným souborům.

## Výstupy

Výchozí výstupní složka je `gui_outputs/`.

Aplikace ukládá:

- anotovaný obrázek nebo video
- JSON metadata s použitým modelem, parametry a shrnutím detekcí
- u videa také náhled prvního anotovaného snímku

## Poznámky

- Na Windows bývá `tkinter` součástí standardní instalace Pythonu.
- Pokud výstupní video nepůjde zapsat jako `.mp4`, backend automaticky zkusí fallback kodek a případně uloží `.avi`.
- Nejrychlejší start v tomto projektu obvykle zajistí model `trained-model.pt`.

