# OpenSCAD 3D Printable Models for PST

This directory contains OpenSCAD models designed for 3D printing to support development teams using the PST (Project Setup Tool). These models provide practical solutions for organizing workspaces, labeling equipment, and improving ergonomics.

## Quick Links

- **[Visual Guide](VISUAL_GUIDE.md)** - See ASCII diagrams and use cases for each model
- **[Quick Reference](QUICK_REFERENCE.md)** - Fast parameter lookup and troubleshooting
- **[Config File](openscad/config.scad)** - Centralized configuration for all models

## Available Models

### 1. Cable Organizer (`cable_organizer.scad`)
**Purpose:** Keep development cables organized and accessible

**Features:**
- 4 separate channels for different cable types
- Mounting holes for desk or rack mounting
- Integrated label area
- Customizable channel sizes

**Customization Parameters:**
- `base_width`: Width of the organizer (default: 80mm)
- `base_length`: Length of the organizer (default: 100mm)
- `num_channels`: Number of cable channels (default: 4)
- `channel_width`: Width of each channel (default: 15mm)
- `channel_height`: Height of channels (default: 20mm)

**Print Settings:**
- Material: PLA or PETG
- Infill: 20-30%
- Supports: Not required
- Print time: ~3-4 hours

---

### 2. Project Tag Holder (`project_tag_holder.scad`)
**Purpose:** Label equipment, projects, or team resources

**Features:**
- Slot for paper labels or printed tags
- Multiple mounting options: magnet, clip, or screw holes
- Debossed text area for permanent markers
- Rounded corners for safety

**Customization Parameters:**
- `tag_width`: Width of tag (default: 60mm)
- `tag_height`: Height of tag (default: 40mm)
- `mounting_type`: "magnet", "clip", or "hole" (default: "magnet")
- `magnet_diameter`: Diameter of magnet recess (default: 10mm)

**Print Settings:**
- Material: PLA
- Infill: 30%
- Supports: Not required (except for "clip" variant)
- Print time: ~1 hour

**Note:** For magnet variant, you'll need 2x 10mm x 3mm neodymium magnets per tag.

---

### 3. Desk Nameplate (`desk_nameplate.scad`)
**Purpose:** Display team member names or project identification

**Features:**
- Angled design for better visibility
- Slot for business card or printed nameplate
- Optional PST branding
- Stable base design

**Customization Parameters:**
- `nameplate_length`: Length of nameplate (default: 150mm)
- `nameplate_width`: Width of base (default: 40mm)
- `back_height`: Height of back panel (default: 50mm)
- `angle`: Viewing angle (default: 15°)

**Print Settings:**
- Material: PLA or PETG
- Infill: 20%
- Supports: Not required
- Print time: ~2 hours

---

### 4. Small Parts Organizer (`small_parts_organizer.scad`)
**Purpose:** Organize USB drives, SD cards, adapters, and dev tools

**Features:**
- 6 compartments (2x3 grid)
- Individual label areas
- Low profile for easy access
- Stackable design

**Customization Parameters:**
- `organizer_width`: Total width (default: 100mm)
- `organizer_length`: Total length (default: 120mm)
- `rows`: Number of rows (default: 2)
- `cols`: Number of columns (default: 3)
- `compartment_height`: Height of walls (default: 15mm)

**Print Settings:**
- Material: PLA
- Infill: 15-20%
- Supports: Not required
- Print time: ~2-3 hours

---

### 5. Laptop Stand (`laptop_stand.scad`)
**Purpose:** Ergonomic stand for development workstations

**Features:**
- Angled platform for better ergonomics
- Cooling ventilation holes
- Cable management cutout
- Non-slip pads
- Accommodates most laptops (up to 15")

**Customization Parameters:**
- `stand_width`: Width to fit laptop (default: 280mm)
- `stand_depth`: Depth of stand (default: 200mm)
- `stand_height`: Height at back (default: 80mm)
- `front_height`: Height at front (default: 20mm)
- `vent_hole_diameter`: Size of cooling holes (default: 8mm)

**Print Settings:**
- Material: PETG or ABS (for heat resistance)
- Infill: 30-40%
- Supports: Required for angled platform
- Print time: ~8-12 hours

**Safety Note:** Ensure adequate ventilation around laptop when using this stand.

---

## Getting Started

### Prerequisites
- [OpenSCAD](https://openscad.org/) installed on your system
- 3D printer with PLA/PETG capability
- Slicer software (Cura, PrusaSlicer, etc.)

### How to Use

1. **Open the model in OpenSCAD:**
   ```bash
   openscad cable_organizer.scad
   ```

2. **Customize parameters:**
   - Edit the parameter values at the top of the file
   - Press F5 to preview changes
   - Press F6 to render the final model

3. **Export for 3D printing:**
   - Click Design → Render (F6)
   - Click File → Export → Export as STL
   - Save the STL file

4. **Slice and print:**
   - Import STL into your slicer
   - Configure print settings as recommended above
   - Generate G-code and print

### Customization Tips

- **Global Configuration:** Use the `config.scad` file to set parameters for all models at once
  - Edit `openscad/config.scad` with your preferences
  - Uncomment the `use <config.scad>` line in individual model files
  - All models will use consistent sizing and branding
- **Scaling:** All models can be scaled in your slicer if you need different sizes
- **Colors:** Print in different colors for organizational purposes
- **Multi-material:** Some models can benefit from multi-color prints for labels
- **Layer height:** Use 0.2mm layer height for general prints, 0.1mm for finer details

## Assembly Notes

### Cable Organizer
- Can be mounted using M4 screws through the corner holes
- Use double-sided tape as an alternative to drilling

### Project Tag Holder (Magnet variant)
- Insert 10mm x 3mm neodymium magnets into recesses
- Use super glue to secure magnets if needed
- Ensure correct polarity for magnetic mounting

### Laptop Stand
- Add rubber feet or silicone pads to the non-slip bumps for extra grip
- Test stability with your specific laptop before regular use

## Contributing

If you create variations or improvements to these models:
1. Keep the parametric design approach
2. Document new parameters clearly
3. Test print before submitting
4. Update this README with your changes

## License

These models are provided under the same license as the PST project. See the main LICENSE file for details.

## Support

For issues or questions about these 3D models:
- Open an issue in the PST repository
- Include photos of print results if relevant
- Specify your printer model and settings

## Disclaimer

Print quality and structural integrity depend on your specific printer, materials, and settings. Always inspect printed parts before use, especially for the laptop stand which supports electronic equipment.

---

**Happy Printing! 🖨️**
