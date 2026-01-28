// Project ID Tag Holder
// For labeling equipment, projects, or team resources

// Parameters
tag_width = 60;
tag_height = 40;
tag_thickness = 2;
slot_width = 50;
slot_height = 25;
corner_radius = 3;
mounting_type = "magnet"; // "magnet", "clip", or "hole"
magnet_diameter = 10;
magnet_depth = 3;

// Rounded rectangle module
module rounded_rectangle(width, height, radius) {
    hull() {
        translate([radius, radius, 0])
            cylinder(r = radius, h = tag_thickness, $fn = 32);
        translate([width - radius, radius, 0])
            cylinder(r = radius, h = tag_thickness, $fn = 32);
        translate([radius, height - radius, 0])
            cylinder(r = radius, h = tag_thickness, $fn = 32);
        translate([width - radius, height - radius, 0])
            cylinder(r = radius, h = tag_thickness, $fn = 32);
    }
}

// Main tag holder
module project_tag_holder() {
    difference() {
        // Base tag
        rounded_rectangle(tag_width, tag_height, corner_radius);
        
        // Slot for label
        translate([(tag_width - slot_width) / 2, 
                   (tag_height - slot_height) / 2, 
                   tag_thickness - 1]) {
            cube([slot_width, slot_height, 2]);
        }
        
        // Text area (debossed)
        translate([(tag_width - slot_width) / 2 + 2, 
                   (tag_height - slot_height) / 2 + 2, 
                   tag_thickness - 0.5]) {
            cube([slot_width - 4, slot_height - 4, 1]);
        }
        
        // Mounting based on type
        if (mounting_type == "magnet") {
            // Magnet recess
            translate([tag_width / 2, tag_height - magnet_diameter / 2 - 3, 0]) {
                cylinder(d = magnet_diameter, h = magnet_depth, $fn = 32);
            }
            translate([tag_width / 2, magnet_diameter / 2 + 3, 0]) {
                cylinder(d = magnet_diameter, h = magnet_depth, $fn = 32);
            }
        } else if (mounting_type == "hole") {
            // Mounting holes
            translate([tag_width / 2, tag_height - 5, -1]) {
                cylinder(d = 3, h = tag_thickness + 2, $fn = 32);
            }
        }
    }
    
    // Clip mount (if selected)
    if (mounting_type == "clip") {
        translate([tag_width / 2 - 8, tag_height, 0]) {
            difference() {
                cube([16, 8, tag_thickness]);
                translate([8, 8, tag_thickness / 2]) {
                    rotate([90, 0, 0]) {
                        cylinder(d = 4, h = 10, $fn = 32);
                    }
                }
            }
        }
    }
}

// Render the tag holder
project_tag_holder();
