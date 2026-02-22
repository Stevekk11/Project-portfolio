## Autor a licence
### "CUDA not available" (GPU zpracování)
Skript bude používat CPU. Pro GPU zpracování nainstalujte PyTorch s CUDA podporou:
- 2GB+ RAM (pro menší modely)
- GPU je doporučena pro rychlejší zpracování (CUDA kompatibilní GPU)

## Poznámky

- První spuštění stáhne vybraný YOLOv8 model (~50-250MB v závislosti na modelu)
- Zpracování obrázků bez GPU se může zdát pomalé
- Obrázky by měly být v běžných formátech (JPG, PNG, BMP, GIF, TIFF, WebP)
result = annotator.annotate_image('path/to/image.jpg', save_annotated=True)
# Inicializace
### ImageAnnotator třída
## Hlavní funkce
# Image Annotator - YOLOv8 Script

Tento skript anotuje obrázky z libovolné složky pomocí YOLOv8 modelu a mapuje detekované objekty na vlastní názvy.

## Mapování tříd (Class Mapping)

- `bus` → `autobus_pid`
- `train` → `tramvaj_pid`


```bash
pip install -r requirements.txt
```

**Poznámka:** Instalace PyTorchu a ultralytics může trvat delší dobu v závislosti na vašem systému.

### Doporučené pro GPU zrychlení:
```bash
pip install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu118
```

## Kompletní workflow pro trénování modelu

### Krok 1: Anotace obrázků

```bash
python image_annotator.py "C:\cesta\ke\obrazkum"
```

## Použití
```

### Krok 4: Trénování modelu

```bash
python train_model.py "C:\cesta\k\dataset.yaml"
```

Interaktivně si můžete vybrat:
- Základní model (nano, small, medium, large, x-large)
- Počet epoch (default: 100)
- Batch size (default: 16)
- Device (0=GPU, -1=CPU)
- Název tréninku

**Příklad:**
```bash
python train_model.py "C:\Users\Steve\dataset\dataset.yaml"
```

Výstup se uloží do `runs/detect/bus_train_model/`

### Krok 5: Použití natrénovaného modelu

```python
from ultralytics import YOLO

# Načtení natrénovaného modelu
model = YOLO('runs/detect/bus_train_model/weights/best.pt')

# Inference na obrázku
results = model.predict(source='image.jpg', conf=0.5)

# Inference na videu
results = model.predict(source='video.mp4')

# Inference na mapě obrázků
results = model.predict(source='folder_with_images/')
```

## Struktura anotací ve formátu YOLO

Každý soubor `image_name.txt` obsahuje řádky ve formátu:
```
<class_id> <x_center> <y_center> <width> <height>
```

Příklad:
```
0 0.5 0.5 0.3 0.4
1 0.2 0.3 0.1 0.2
```

Kde:
- `0` = autobus_pid
- `1` = tramvaj_pid
- Souřadnice jsou normalizované na 0-1 (relativně k šířce/výšce obrázku)

## Příklad Python skriptu pro trénování

```python
from train_model import YOLOv8Trainer

# Vytvořit trenéra
trainer = YOLOv8Trainer(base_model='yolov8n.pt')

# Trénovat model
### Metoda 1: Interaktivní režim (s otázkami)
)

# Predikce na novém obrázku
results = trainer.predict(
    model_path='runs/detect/my_bus_model/weights/best.pt',
    source='new_image.jpg',
    conf=0.5
)
```

## Dostupné modely YOLOv8

### Pro anotaci (detekci):

python image_annotator.py
- **yolov8s.pt** (small) - Vyvážený (~22.5MB)
Skript vás postupně vyzve k zadání:
1. Cesty ke složce s obrázky
2. Zvolení výstupního adresáře
3. Volby rekurzivního zpracování podsložek
4. Výběru modelu YOLOv8 (nano, small, medium, large, x-large)
### Metoda 2: Parametry příkazové řádky

python image_annotator.py "C:/cesta/ke/slozce/s/obrazky"
## Výstup

**Pro trénování:**
- Doporučeno: **nano** nebo **small** (kratší trénink, vyhovující přesnost)
- Máte-li výkonný GPU: **medium** (lepší přesnost)

## Pokyny pro úspěšný trénink

### 1. Datová sada

Skript vytvoří:
- Pestré podmínky: různé úhly, světlo, vzdálenosti
1. **Anotované obrázky**: Obrázky s nakreslenými bounding boxy a názvem třídy
2. **JSON soubor s výsledky**: `annotations.json`
   - Obsahuje všechny detekce se souřadnicemi a spolehlivostí
## Příklad výstupu JSON
- Zkontrolujte anotované obrázky - jsou-li detekce správné
- Pokud je málo detektů, zvyšte confidence threshold

### 3. Trénování
```json
[
  {
    "image": "C:/path/to/image.jpg",
    "detections": [
      {
        "class_id": 5,
        "class_name": "bus",
        "class_name_mapped": "autobus_pid",
        "confidence": 0.85,
        "bbox": {
          "x_min": 100.5,
          "y_min": 50.3,
          "x_max": 400.2,
          "y_max": 350.8
        }
      }
    ],
    "num_detections": 1,
    "error": null
  }
]

# Anotace celé složky
# Anotace složky s YOLO formátem pro trénink
results = annotator.annotate_folder(
    'path/to/folder',
    'folder_with_images',
    save_annotated=True,
    output_dir='output_folder',
    save_json=True,
    save_yolo=True,           # Uložit YOLO format
    create_dataset=True,      # Vytvořit dataset strukturu
    recursive=False
)
```

## Parametry annotate_folder()
### Trénování modelu

```python
from train_model import YOLOv8Trainer

    data_yaml='dataset.yaml',
    epochs=100,
    batch_size=16,
    device=0,              # 0=GPU, -1=CPU
    name='my_model'
)

- **folder_path** (str): Cesta ke složce s obrázky
- **save_annotated** (bool): Uložit anotované obrázky (default: True)
- **output_dir** (str): Adresář pro výstupní soubory (default: stejný jako vstupní)
- **save_json** (bool): Uložit JSON soubor s výsledky (default: True)
- **recursive** (bool): Zpracovat podsložky (default: False)
# Validace
metrics = trainer.validate(
    model_path='runs/detect/my_model/weights/best.pt',
    data_yaml='dataset.yaml'
)

# Inference
predictions = trainer.predict(
    model_path='runs/detect/my_model/weights/best.pt',
   ```
3. **Console výstup**: Reálné aktualizace v terminálu

## Vady a chyby

### Během anotace

- Skript loguje všechny chyby do konzole
- Pokud se obrázek nepodaří zpracovat, pokračuje se dalším
- Pro rychlost: **nano** nebo **small**
- Pro vyvážení: **medium**
- Pro maximální přesnost: **large** nebo **x-large**
- Přidejte více kvalitních anotovaných obrázků
- Ověřte, že anotace jsou správné
- Používejte Data Augmentation (já ho zapínám automaticky)

## Příslušné soubory

```
PyCharmMiscProject/
├── image_annotator.py          # Anotace obrázků
├── train_model.py              # Trénování modelu
├── create_training_config.py   # Vytvoření YAML
├── requirements.txt            # Závislosti
├── README_ANNOTATOR.md         # Tato dokumentace
└── bus_photos_backward/        # Vaše obrázky
```

## Další zdroje

Vytvořeno jako nástroj pro anotaci obrázků autobusů a tramvají.
- [YOLOv8 Dokumentace](https://docs.ultralytics.com)
- [YOLOv8 Training Guide](https://docs.ultralytics.com/modes/train/)
- [Roboflow Dataset Tool](https://roboflow.com) - Pro správu dat

## Poznámky

- Čím více kvalitních anotačních obrázků, tím lepší model
- Trénování je nejpomalejší část (0.5-2 hodiny na GPU)
- Inferenční čas s natrénovaným modelem je stejně rychlý jako s předtrénovaným
- Uložte si nejlepší model (`best.pt`) - je to vaš finální model

