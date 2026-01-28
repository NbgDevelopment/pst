// Desk Nameplate Holder
// For team member names or project identification

// Parameters
nameplate_length = 150;
nameplate_width = 40;
base_thickness = 3;
back_height = 50;
back_thickness = 3;
angle = 15; // Tilt angle for better visibility
slot_depth = 2;
slot_height = 30;

// Main nameplate holder
module desk_nameplate() {
    difference() {
        union() {
            // Base
            cube([nameplate_length, nameplate_width, base_thickness]);
            
            // Angled back support
            translate([0, nameplate_width - back_thickness, base_thickness]) {
                rotate([angle, 0, 0]) {
                    difference() {
                        cube([nameplate_length, back_thickness, back_height]);
                        
                        // Card slot
                        translate([10, -1, (back_height - slot_height) / 2]) {
                            cube([nameplate_length - 20, 
                                  slot_depth + 2, 
                                  slot_height]);
                        }
                    }
                }
            }
            
            // Side supports
            for (x = [0, nameplate_length - 5]) {
                translate([x, 0, base_thickness]) {
                    linear_extrude(height = 5) {
                        polygon([[0, 0], 
                                [5, 0], 
                                [5, nameplate_width * 0.6], 
                                [0, nameplate_width * 0.4]]);
                    }
                }
            }
        }
        
        // Optional logo area (debossed)
        translate([nameplate_length - 35, nameplate_width / 2, 0]) {
            linear_extrude(height = 1.5) {
                text("PST", size = 8, halign = "center", valign = "center", 
                     font = "Liberation Sans:style=Bold");
            }
        }
    }
}

// Render the nameplate
desk_nameplate();
