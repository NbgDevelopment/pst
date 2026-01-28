// Small Parts Organizer
// For organizing USB drives, SD cards, adapters, and other small dev tools

// Parameters
organizer_width = 100;
organizer_length = 120;
base_height = 2;
wall_thickness = 2;
compartment_height = 15;

// Grid layout
rows = 2;
cols = 3;

// Main organizer module
module small_parts_organizer() {
    compartment_width = (organizer_width - wall_thickness * (cols + 1)) / cols;
    compartment_length = (organizer_length - wall_thickness * (rows + 1)) / rows;
    
    difference() {
        union() {
            // Base
            cube([organizer_length, organizer_width, base_height]);
            
            // Outer walls
            // Front and back
            for (y = [0, organizer_width - wall_thickness]) {
                translate([0, y, base_height]) {
                    cube([organizer_length, wall_thickness, compartment_height]);
                }
            }
            
            // Left and right
            for (x = [0, organizer_length - wall_thickness]) {
                translate([x, 0, base_height]) {
                    cube([wall_thickness, organizer_width, compartment_height]);
                }
            }
            
            // Internal dividers - vertical (along length)
            for (i = [1:rows-1]) {
                translate([0, wall_thickness + i * (compartment_width + wall_thickness), base_height]) {
                    cube([organizer_length, wall_thickness, compartment_height]);
                }
            }
            
            // Internal dividers - horizontal (along width)
            for (j = [1:cols-1]) {
                translate([wall_thickness + j * (compartment_length + wall_thickness), 0, base_height]) {
                    cube([wall_thickness, organizer_width, compartment_height]);
                }
            }
            
            // Label areas on outer walls
            for (i = [0:rows-1]) {
                for (j = [0:cols-1]) {
                    x_pos = wall_thickness + j * (compartment_length + wall_thickness) + compartment_length / 2;
                    y_pos = wall_thickness + i * (compartment_width + wall_thickness);
                    
                    translate([x_pos - 15, y_pos - 1, base_height + compartment_height - 5]) {
                        cube([30, 1.5, 3]);
                    }
                }
            }
        }
        
        // Rounded corners on outer edges (optional decoration)
        for (x = [0, organizer_length]) {
            for (y = [0, organizer_width]) {
                translate([x, y, -1]) {
                    cylinder(r = 1, h = base_height + 2, $fn = 16);
                }
            }
        }
    }
}

// Render the organizer
small_parts_organizer();
