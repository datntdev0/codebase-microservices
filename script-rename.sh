#!/bin/bash
# Bash script to rename AdministrationService to AdminService

# Define the paths and strings from command line arguments
OLD_NAME="$1"
NEW_NAME="$2"
SOLUTION_DIR="$3"
SCRIPT_NAME=$(basename "$0")

# Create non-dotted versions of names
OLD_NAME_NO_DOTS="${OLD_NAME//./}"
NEW_NAME_NO_DOTS="${NEW_NAME//./}"

# File extensions to process - could be moved to a config file
EXTENSIONS=(
    "cs"
    "csproj"
    "json"
    "sln"
    "cshtml"
    "html"
    "ts"
    "razor"
    "xml"
    "config"
    "yml"
    "yaml"
    "md"
    "ps1"
    "azure"
    "local"
    "sh"
)

# Step 1: Rename project files containing the old name (both dotted and non-dotted versions)
# Find files recursively in all subdirectories
find "$SOLUTION_DIR" -type f \( -name "*$OLD_NAME*" -o -name "*$OLD_NAME_NO_DOTS*" \) | sort -r | while read file; do
    filename=$(basename "$file")
    new_filename="$filename"
    
    # Replace dotted version
    if [[ "$filename" == *"$OLD_NAME"* ]]; then
        new_filename="${filename//$OLD_NAME/$NEW_NAME}"
    fi
    
    # Replace non-dotted version
    if [[ "$filename" == *"$OLD_NAME_NO_DOTS"* ]]; then
        new_filename="${filename//$OLD_NAME_NO_DOTS/$NEW_NAME_NO_DOTS}"
    fi
    
    if [ "$filename" != "$new_filename" ]; then
        new_filepath="$(dirname "$file")/$new_filename"
        echo "Renaming file: $file to $new_filepath"
        mv "$file" "$new_filepath"
    fi
done

# Step 2: Find all directories with old name and rename them (both dotted and non-dotted versions)
find "$SOLUTION_DIR" -type d \( -name "*$OLD_NAME*" -o -name "*$OLD_NAME_NO_DOTS*" \) | sort -r | while read dir; do
    dirname=$(basename "$dir")
    new_dirname="$dirname"
    
    # Replace dotted version
    if [[ "$dirname" == *"$OLD_NAME"* ]]; then
        new_dirname="${dirname//$OLD_NAME/$NEW_NAME}"
    fi
    
    # Replace non-dotted version
    if [[ "$dirname" == *"$OLD_NAME_NO_DOTS"* ]]; then
        new_dirname="${dirname//$OLD_NAME_NO_DOTS/$NEW_NAME_NO_DOTS}"
    fi
    
    if [ "$dirname" != "$new_dirname" ]; then
        new_dirpath="$(dirname "$dir")/$new_dirname"

        if [ ! -d "$new_dirpath" ]; then
            echo "Renaming directory: $dir to $new_dirpath"
            mv "$dir" "$new_dirpath"
        else
            echo "Target directory already exists: $new_dirpath. Moving contents from $dir to $new_dirpath"
            
            # Move all files and subdirectories from old directory to new directory
            find "$dir" -mindepth 1 -maxdepth 1 | while read item; do
                item_name=$(basename "$item")
                target_path="$new_dirpath/$item_name"
                
                if [ -e "$target_path" ] && [ -d "$item" ] && [ -d "$target_path" ]; then
                    echo "Directory already exists in target, merging contents: $target_path"
                    # Recursively move contents from subdirectory
                    find "$item" -mindepth 1 -maxdepth 1 | while read subitem; do
                        subitem_name=$(basename "$subitem")
                        subtarget="$target_path/$subitem_name"
                        
                        if [ ! -e "$subtarget" ]; then
                            echo "Moving subitem: $subitem to $target_path/"
                            mv "$subitem" "$target_path/"
                        else
                            echo "Item already exists in target subdirectory, skipping: $subtarget"
                        fi
                    done
                elif [ ! -e "$target_path" ]; then
                    echo "Moving item: $item to $new_dirpath/"
                    mv "$item" "$new_dirpath/"
                else
                    echo "Item already exists in target, skipping: $target_path"
                fi
            done
            
            # Remove the directory if it's empty
            if [ -z "$(ls -A "$dir" 2>/dev/null)" ]; then
                echo "Removing empty directory: $dir"
                rmdir "$dir"
            fi
        fi
    fi
done

# Step 3: Update file contents
echo "Processing all file contents..."

# Process files with extensions
for ext in "${EXTENSIONS[@]}"; do
    find "." -type f -name "*.$ext" -not -name "$SCRIPT_NAME" | while read file; do
        if grep -q "$OLD_NAME" "$file" || grep -q "$OLD_NAME_NO_DOTS" "$file"; then
            # Replace both dotted and non-dotted versions, but skip if the old name contains "EntityFramework"
            if [[ "$OLD_NAME" != *"EntityFramework"* && "$OLD_NAME" != *"MongoDB"* ]]; then
                sed -i "s/$OLD_NAME/$NEW_NAME/g" "$file"
                sed -i "s/$OLD_NAME_NO_DOTS/$NEW_NAME_NO_DOTS/g" "$file"
            fi
            
            # Special handling for paths in solution and project files
            if [[ "$file" == *.sln || "$file" == *.csproj ]]; then
                sed -i "s|services/$OLD_FOLDER_NAME|services/$NEW_FOLDER_NAME|g" "$file"
                sed -i "s|services\\\\$OLD_FOLDER_NAME|services\\\\$NEW_FOLDER_NAME|g" "$file"
            fi
            
            echo "Updated file: $file"
        fi
    done
done

# Then, process Dockerfiles (which typically have no extension)
find "." -type f -name "Dockerfile" | while read file; do
    if grep -q "$OLD_NAME" "$file" || grep -q "$OLD_NAME_NO_DOTS" "$file"; then
        sed -i "s/$OLD_NAME/$NEW_NAME/g" "$file"
        sed -i "s/$OLD_NAME_NO_DOTS/$NEW_NAME_NO_DOTS/g" "$file"
        echo "Updated Dockerfile: $file"
    fi
done

echo "Renaming process completed! Please check the output and run the solution to verify everything works correctly." 