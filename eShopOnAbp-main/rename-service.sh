#!/bin/bash
# Bash script to rename AdministrationService to AdminService

# Define the paths and strings
SOLUTION_DIR="."  # Assuming script runs from solution root
OLD_NAME="AdministrationService"
NEW_NAME="AdminService"
OLD_FOLDER_NAME="administration"
NEW_FOLDER_NAME="admin"
SCRIPT_NAME=$(basename "$0")  # Get the name of this script

# Step 1: Rename project directories
OLD_PATH="$SOLUTION_DIR/services/$OLD_FOLDER_NAME"
NEW_PATH="$SOLUTION_DIR/services/$NEW_FOLDER_NAME"

# Check if old directory exists
if [ -d "$OLD_PATH" ]; then
    # Create parent directory if it doesn't exist
    mkdir -p "$(dirname "$NEW_PATH")"
    
    # Remove existing directory at new path if it exists
    if [ -d "$NEW_PATH" ]; then
        echo "Removing existing directory at $NEW_PATH"
        rm -rf "$NEW_PATH"
    fi
    
    # Move the directory instead of copying
    echo "Moving directory from $OLD_PATH to $NEW_PATH"
    mv "$OLD_PATH" "$NEW_PATH"
fi

# Step 2: Rename project files containing the old name
# Process files from the deepest directories first to avoid path issues
find "$NEW_PATH" -type f | sort -r | while read file; do
    filename=$(basename "$file")
    if [[ "$filename" == *"$OLD_NAME"* ]]; then
        new_filename="${filename//$OLD_NAME/$NEW_NAME}"
        new_filepath="$(dirname "$file")/$new_filename"
        echo "Renaming file: $file to $new_filepath"
        mv "$file" "$new_filepath"
    fi
done

# Step 3: Find all directories with old name and rename them (starting with the deepest ones)
find "$NEW_PATH" -type d | sort -r | while read dir; do
    dirname=$(basename "$dir")
    if [[ "$dirname" == *"$OLD_NAME"* ]]; then
        new_dirname="${dirname//$OLD_NAME/$NEW_NAME}"
        new_dirpath="$(dirname "$dir")/$new_dirname"
        echo "Renaming directory: $dir to $new_dirpath"
        mv "$dir" "$new_dirpath"
    fi
done

# Step 4: Update file contents - completely consolidated processing
echo "Processing all files..."

# First, process files with extensions
for ext in cs csproj json sln cshtml html ts razor xml config yml yaml md ps1 azure local sh; do
    find "$SOLUTION_DIR" -type f -name "*.$ext" -not -name "$SCRIPT_NAME" | while read file; do
        if grep -q "$OLD_NAME" "$file" || grep -q "AdministrationService" "$file"; then
            sed -i "s/$OLD_NAME/$NEW_NAME/g" "$file"
            sed -i "s/AdministrationService/AdminService/g" "$file"
            
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
find "$SOLUTION_DIR" -type f -name "Dockerfile" | while read file; do
    if grep -q "$OLD_NAME" "$file" || grep -q "AdministrationService" "$file"; then
        sed -i "s/$OLD_NAME/$NEW_NAME/g" "$file"
        sed -i "s/AdministrationService/AdminService/g" "$file"
        echo "Updated Dockerfile: $file"
    fi
done

echo "Renaming process completed! Please check the output and run the solution to verify everything works correctly." 