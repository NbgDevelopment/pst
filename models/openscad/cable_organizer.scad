// Cable Organizer for Desk or Rack Mount
// Helps keep development cables organized and labeled

// Parameters
wall_thickness = 2;
base_width = 80;
base_length = 100;
base_height = 2;
channel_width = 15;
channel_height = 20;
num_channels = 4;
mounting_hole_diameter = 4;

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
