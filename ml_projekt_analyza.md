## Návrh projektu - detekce nových zastávek MHD v Praze a jejich vybavení
## Cíl projektu

### Cílem tohoto projektu je vytvořit systém pro detekci nových zastávek MHD v Praze a analyzovat jejich vybavení.
- Projekt bude detekovat nové zastávky MHD v Praze (přístřešek, koš, název zastávky)
- Využití projektu: objevení, kterým zastávkám chybí vybavení, a které zastávky jsou nově zřízené.
- Model bude vytrénován v pythonu pomocí knihovny yolo, pytorch, atd. a vlastních fotek zastávek MHD v Praze.
- Sběr dat bude probíhat pomocí vlastních fotografií zastávek MHD v Praze a fotografií z map Google, které budou následně anotovány pro trénink modelu pomocí Roboflow.
- Trénování modelu bude probíhat v Google Colab.
- Hotová aplikace bude schopna detekovat nové zastávky pomocí webkamery, nebo standardního vstupu z fotek, a bude zobrazovat informace o vybavení zastávky (přístřešek, koš, název zastávky - pokud tam nějaký bude).