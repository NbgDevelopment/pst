// Laptop Stand with Cooling Ventilation
// Ergonomic stand for development workstations

// Parameters
stand_width = 280;  // Width to accommodate most laptops
stand_depth = 200;
stand_height = 80;  // Height at the back
front_height = 20;  // Height at the front
wall_thickness = 3;
vent_hole_diameter = 8;
vent_spacing = 15;

// Main laptop stand
module laptop_stand() {
    difference() {
        union() {
            // Main angled platform
            hull() {
                // Front edge
                translate([0, 0, 0])
                    cube([stand_width, wall_thickness, front_height]);
                
                // Back edge
                translate([0, stand_depth - wall_thickness, 0])
                    cube([stand_width, wall_thickness, stand_height]);
            }
            
            // Side walls for stability
            for (x = [0, stand_width - wall_thickness]) {
                translate([x, 0, 0]) {
                    linear_extrude(height = wall_thickness) {
                        polygon([
                            [0, 0],
                            [wall_thickness, 0],
                            [wall_thickness, stand_depth],
                            [0, stand_depth]
                        ]);
                    }
                }
            }
            
            // Front lip to prevent sliding
            translate([wall_thickness, 0, 0])
                cube([stand_width - 2 * wall_thickness, 10, front_height + 5]);
            
            // Rear support structure
            translate([0, stand_depth - 30, 0])
                cube([stand_width, 30, wall_thickness]);
        }
        
        // Cooling ventilation holes
        for (x = [20:vent_spacing:stand_width-20]) {
            for (y = [20:vent_spacing:stand_depth-40]) {
                translate([x, y, -1]) {
                    cylinder(d = vent_hole_diameter, h = stand_height + 2, $fn = 32);
                }
            }
        }
        
        // Cable management cutout at the back
        translate([stand_width / 2 - 20, stand_depth - wall_thickness - 1, stand_height / 2]) {
            cube([40, wall_thickness + 2, 30]);
        }
        
        // Branding (optional)
        translate([stand_width / 2, 5, front_height + 2]) {
            rotate([90, 0, 0]) {
                linear_extrude(height = 1) {
                    text("PST", size = 10, halign = "center", 
                         font = "Liberation Sans:style=Bold");
                }
            }
        }
    }
    
    // Non-slip pads (raised bumps)
    for (x = [15, stand_width - 15]) {
        for (y = [15, stand_depth - 15]) {
            translate([x, y, 0]) {
                cylinder(d = 12, h = 2, $fn = 32);
            }
        }
    }
}

// Render the laptop stand
laptop_stand();
