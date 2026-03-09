from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
TILES_DIR = ROOT / "Assets" / "Art" / "Tiles"
UNITS_DIR = ROOT / "Assets" / "Art" / "Units"
TILES_DIR.mkdir(parents=True, exist_ok=True)
UNITS_DIR.mkdir(parents=True, exist_ok=True)

hex_points = "64,6 192,6 245,128 192,250 64,250 11,128"

palette = {
    "plain": "#92c46c",
    "forest": "#2f7d32",
    "mountain": "#8e8e8e",
    "volcano": "#912f24",
    "desert": "#d8c27a",
    "rocky_tracts": "#6b625f",
    "swamp": "#4b5f2a",
    "tundra": "#b7d5e5",
    "castle": "#d9d9d9",
    "fortress": "#b8b8bf",
    "tower": "#c7c7d6",
    "river": "#4a89c7",
    "fjord": "#2f5f8a",
    "ocean": "#24507a",
}

for name, color in palette.items():
    label = name.replace("_", " ").title()
    svg = f'''<svg xmlns="http://www.w3.org/2000/svg" width="256" height="256" viewBox="0 0 256 256">
  <polygon points="{hex_points}" fill="{color}" stroke="#111" stroke-width="6" />
  <text x="128" y="138" font-size="22" text-anchor="middle" fill="white" font-family="Arial">{label}</text>
</svg>
'''
    (TILES_DIR / f"tile_{name}.svg").write_text(svg, encoding="utf-8")

units = {
    "overlord_1": ("#8a2be2", "M1"),
    "overlord_2": ("#dc143c", "M2"),
    "monster_1": ("#ff8c00", "MON"),
}

for name, (color, label) in units.items():
    svg = f'''<svg xmlns="http://www.w3.org/2000/svg" width="256" height="256" viewBox="0 0 256 256">
  <circle cx="128" cy="128" r="104" fill="{color}" stroke="#111" stroke-width="6" />
  <text x="128" y="138" font-size="42" text-anchor="middle" fill="white" font-family="Arial">{label}</text>
</svg>
'''
    (UNITS_DIR / f"{name}.svg").write_text(svg, encoding="utf-8")

print("Generated SVG assets")
