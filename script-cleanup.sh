#!/bin/bash
# Set the directory to the current directory (project root)
DIRECTORY="."

# Display script information
echo "Running cleanup script from project root directory..."
echo "Current directory: $(pwd)"

# Define directories to delete
directories=("bin" "obj" "logs" "node_modules", ".vs")
count=0

echo "Deleting specified directories in '$1'..."
echo "Directories to delete: ${directories[@]}"
echo

cd "$1" || exit 1

# Find and delete specified directories
for dir in "${directories[@]}"; do
    while IFS= read -r -d '' dir_path; do
        echo "Deleting: $dir_path"
        rm -rf "$dir_path"
        ((count++))
    done < <(find . -type d -name "$dir" -print0)
done

echo
echo "Number of deleted directories: $count"

echo "Searching for empty directories in '$1'..."

# Use find to locate and remove empty directories recursively
# -depth processes deepest directories first
# -type d only finds directories
# -empty only finds empty directories
# -delete removes them
find "." -depth -type d -empty -delete

echo "Done removing empty directories."