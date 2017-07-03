
# Get the directory that this configuration file exists in
dir = File.dirname(__FILE__)

# Load the sencha-touch framework automatically.
# load File.join(dir, "..", "themes")

# Compass configurations

relative_assets = true

sass_path    = File.join(dir, "SASS")
css_path     = File.join(dir, "CSS")

images_dir	 = "Images"
generated_images_dir = "Images"
images_path	 = File.join(dir, "Images")

environment  = :production

# Use ":compact" if you need to see line numbers from the .scss file for debugging, Use ":compressed" and do a final save or "clean and compile"

output_style = :compact # by Compass.app 
sass_options = {:debug_info=>false} # by Compass.app 
line_comments = true # by Compass.app 