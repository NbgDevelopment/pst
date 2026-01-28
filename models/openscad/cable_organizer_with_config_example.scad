// Example: Cable Organizer Using Config File
// This demonstrates how to use config.scad for consistent customization

// Include the global configuration
// Uncomment the line below to use shared config:
// use <config.scad>

// If using config.scad, these values come from there.
// If not using config, these are the defaults:

// From config (or use these defaults)
wall_thickness = 2;
base_width = 80;
base_length = 100;
base_height = 2;
channel_width = 15;
channel_height = 20;
num_channels = 4;
mounting_hole_diameter = 4;

// Example: To use config values, uncomment use statement above
// Then these would automatically use config.scad values:
// wall_thickness (from config)
// base_width = cable_org_width (from config)
// base_length = cable_org_length (from config)
// num_channels = cable_org_channels (from config)

// Main module
module cable_organizer() {
    difference() {
        union() {
            // Base plate
            cube([base_length, base_width, base_height]);
            
            // Channels for cables
            for (i = [0:num_channels-1]) {
                translate([10 + i * 22, 10, base_height]) {
                    difference() {
                        // Outer channel
                        cube([channel_width + wall_thickness * 2, 
                              base_width - 20, 
                              channel_height]);
                        
                        // Inner channel cutout
                        translate([wall_thickness, wall_thickness, wall_thickness]) {
                            cube([channel_width, 
                                  base_width - 20 - wall_thickness * 2, 
                                  channel_height]);
                        }
                        
                        // Front opening (entrance for cables)
                        translate([wall_thickness, 0, channel_height - 8]) {
                            cube([channel_width, wall_thickness + 1, 10]);
                        }
                    }
                }
            }
            
            // Label area
            translate([5, base_width - 15, base_height]) {
                cube([base_length - 10, 10, 2]);
            }
        }
        
        // Mounting holes
        for (x = [10, base_length - 10]) {
            for (y = [10, base_width - 10]) {
                translate([x, y, -1]) {
                    cylinder(h = base_height + 2, d = mounting_hole_diameter, $fn = 32);
                }
            }
        }
    }
}

// Render the cable organizer
cable_organizer();

// USAGE NOTES:
// 1. To use config.scad: Uncomment the 'use <config.scad>' line at the top
// 2. Then replace the parameter values with config variable names
// 3. All models can share the same configuration
// 4. Update config.scad once to change all models consistently
