# Quick Reference Guide for OpenSCAD Models

This guide provides a quick overview of key customization parameters for each model.

## Cable Organizer
```openscad
// Key Parameters to Customize
base_width = 80;          // Adjust for desk width
base_length = 100;        // Adjust for cable bundle size
num_channels = 4;         // Change number of cable slots (2-6 recommended)
channel_width = 15;       // Wider for thicker cables
channel_height = 20;      // Taller for cable connectors
```

## Project Tag Holder
```openscad
// Key Parameters to Customize
tag_width = 60;           // Standard business card width
tag_height = 40;          // Standard business card height
mounting_type = "magnet"; // Options: "magnet", "clip", "hole"
magnet_diameter = 10;     // Common: 10mm or 12mm magnets
```

## Desk Nameplate
```openscad
// Key Parameters to Customize
nameplate_length = 150;   // Adjust for desk size
back_height = 50;         // Height visible from seated position
angle = 15;               // Viewing angle (10-20° recommended)
slot_height = 30;         // Match your card/label size
```

## Small Parts Organizer
```openscad
// Key Parameters to Customize
rows = 2;                 // Vertical compartments
cols = 3;                 // Horizontal compartments
compartment_height = 15;  // Height of dividers
organizer_width = 100;    // Fit to drawer/desk space
organizer_length = 120;   // Fit to drawer/desk space
```

## Laptop Stand
```openscad
// Key Parameters to Customize
stand_width = 280;        // Laptop width + margins (280-320mm typical)
stand_height = 80;        // Ergonomic lift height
front_height = 20;        // Front edge height
vent_hole_diameter = 8;   // Cooling airflow size
```

## Common Modifications

### Making Models Larger
- Multiply all dimension parameters by the same factor
- Example: 1.5x for 50% larger models

### Making Models Smaller
- Divide all dimension parameters by the same factor
- Ensure wall_thickness remains ≥ 2mm for strength

### Adjusting for Different Printers
- Small printer (< 200mm): Use cable organizer, tag holder, nameplate
- Medium printer (200-250mm): All models except laptop stand
- Large printer (> 250mm): All models including laptop stand

### Material Considerations
- **PLA**: Good for most models, easy to print
- **PETG**: Better for laptop stand (heat resistant)
- **ABS**: Alternative for laptop stand, requires heated bed
- **TPU**: Can be used for non-slip pads on laptop stand

## Print Time Estimates (0.2mm layer, 50mm/s)

| Model | Print Time | Filament |
|-------|-----------|----------|
| Cable Organizer | 3-4 hours | ~50g |
| Project Tag Holder | 1 hour | ~15g |
| Desk Nameplate | 2 hours | ~30g |
| Small Parts Organizer | 2-3 hours | ~40g |
| Laptop Stand | 8-12 hours | ~150g |

## Troubleshooting

**Model too large for printer:**
- Scale down in slicer (80% often works well)
- Split model and glue parts together after printing

**Weak walls/structure:**
- Increase wall_thickness parameter
- Use higher infill percentage (30-40%)
- Add more perimeters in slicer settings

**Poor overhang quality:**
- Enable supports in slicer
- Reduce overhang angles in model parameters
- Use slower print speeds for overhangs

**Warping issues:**
- Use heated bed (60°C for PLA, 80°C for PETG)
- Add brim or raft in slicer
- Ensure good bed adhesion

## Batch Printing Tips

If printing multiple models:
1. Start with tag holders (quick, test printer settings)
2. Then nameplates and organizers
3. Finally larger items like cable organizer
4. Laptop stand requires dedicated print time

Group similar-sized models to maximize bed usage and minimize print time.
