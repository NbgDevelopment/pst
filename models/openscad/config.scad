// Global Configuration for PST 3D Models
// Edit these values to customize all models consistently
// Then use "use" or "include" in individual SCAD files to apply these settings

// === TEAM BRANDING ===
team_name = "PST";  // Change to your team name
team_logo_text = "PST";  // Text for logo areas

// === MATERIAL SETTINGS ===
// Adjust based on your printer's capabilities
wall_thickness = 2;  // Minimum recommended: 2mm for PLA, 2.5mm for PETG
base_thickness = 2;  // Base layer thickness

// === SIZE PREFERENCES ===
// Scale factor for all models (1.0 = normal, 1.5 = 50% larger, 0.8 = 20% smaller)
global_scale = 1.0;

// === MOUNTING PREFERENCES ===
// Choose your preferred mounting method for tag holders
// Options: "magnet", "clip", "hole"
preferred_mounting = "magnet";

// Magnet specifications (if using magnetic mounting)
magnet_diameter = 10;  // Standard sizes: 8mm, 10mm, 12mm
magnet_depth = 3;      // Typical: 3mm

// Screw hole specifications (if using screw mounting)
screw_diameter = 4;    // M3 = 3mm, M4 = 4mm, M5 = 5mm

// === DESK DIMENSIONS ===
// Measure your desk to optimize model sizes
desk_depth = 600;      // Typical desk depth in mm
desk_width = 1200;     // Typical desk width in mm

// === PRINTER SPECIFICATIONS ===
// Your 3D printer's capabilities
max_print_width = 220;   // Max X dimension (mm)
max_print_depth = 220;   // Max Y dimension (mm)
max_print_height = 250;  // Max Z dimension (mm)

// Print settings recommendations
recommended_layer_height = 0.2;  // mm
recommended_infill = 20;         // percent
recommended_print_speed = 50;    // mm/s

// === CABLE ORGANIZER SPECIFIC ===
cable_org_channels = 4;         // Number of cable channels
cable_org_channel_width = 15;   // Width per channel (mm)
cable_org_length = 100;         // Total length (mm)
cable_org_width = 80;           // Total width (mm)

// === TAG HOLDER SPECIFIC ===
tag_width = 60;                 // Business card standard: 60mm
tag_height = 40;                // Business card standard: 40mm

// === NAMEPLATE SPECIFIC ===
nameplate_length = 150;         // Length for desk nameplate
nameplate_angle = 15;           // Viewing angle (degrees)

// === ORGANIZER SPECIFIC ===
organizer_rows = 2;             // Grid rows
organizer_cols = 3;             // Grid columns
organizer_compartment_height = 15;  // Height of dividers

// === LAPTOP STAND SPECIFIC ===
laptop_width = 280;             // Accommodate laptop width + margins
laptop_stand_height = 80;       // Back height for ergonomics
laptop_stand_front_height = 20; // Front height
laptop_vent_diameter = 8;       // Cooling hole diameter

// === COLOR RECOMMENDATIONS ===
// Note: These are for reference when multi-color printing
// OpenSCAD doesn't use these, but your slicer might
primary_color = "blue";         // Main body color
accent_color = "white";         // Labels and text
highlight_color = "yellow";     // Important markings

// === ADVANCED OPTIONS ===
// Enable/disable features
enable_branding = true;         // Show team name/logo
enable_decorative_features = true;  // Rounded corners, etc.
enable_mounting_holes = true;   // Add mounting holes

// Precision settings
fn_quality = 32;  // $fn value for cylinders (16=draft, 32=normal, 64=high quality)

// === HELPER FUNCTIONS ===
// Use these in your models for consistent dimensions

function scaled(value) = value * global_scale;

function grid_dimension(total, dividers, spacing) = 
    (total - spacing * (dividers + 1)) / dividers;

// === USAGE INSTRUCTIONS ===
// To use this config in your SCAD file:
// 1. Add this line at the top: use <../config.scad>
// 2. Reference variables like: cable_org_channels
// 3. Update this file to change all models consistently

// === PRINT RECOMMENDATIONS ===
/*
Based on your printer size:
- Small printer (<200mm): Print tag holders, nameplates first
- Medium printer (200-250mm): Can print all except laptop stand
- Large printer (>250mm): Can print all models

Recommended print order for testing:
1. Project Tag Holder (1 hour) - Test quality
2. Desk Nameplate (2 hours) - Test layer adhesion  
3. Small Parts Organizer (2-3 hours) - Test wall strength
4. Cable Organizer (3-4 hours) - Test overall quality
5. Laptop Stand (8-12 hours) - Final production print
*/
