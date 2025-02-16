import * as fs from 'fs';

const packageJson = JSON.parse(fs.readFileSync('package.json', 'utf8'));
const version: string = packageJson.version;
const versionRegex = /Include="Purview\.Telemetry\.SourceGenerator" Version="([\d]+\.[\d]+\.[\d]+(?:-[a-zA-Z0-9.]+)?(?:\+[a-zA-Z0-9.]+)?)"/;

// Define the list of files to update
const filesToUpdate: string[] = [
    'README.md',
    '.wiki/Home.md',
    'samples/SampleApp/SampleApp.Host/SampleApp.Host.csproj'
];

// Function to update version in specific files
function updateFilesVersion() {
	console.log(`🚀 Updating version to ${version}...`);
    
	filesToUpdate.forEach(file => {
        if (fs.existsSync(file)) {
            let content = fs.readFileSync(file, 'utf8');

            // Extract the current version from the file
            const match = content.match(versionRegex);
            if (match) {
                const currentVersion = match[1];

                // Check if the current version is already up-to-date
                if (currentVersion === version) {
                    console.log(`  🔹 Skipping ${file}: Already up-to-date (${currentVersion})`);
                    return;
                }

                const updatedContent = content.replace(versionRegex, `Include="Purview.Telemetry.SourceGenerator" Version="${version}"`);
                fs.writeFileSync(file, updatedContent, 'utf8');
                console.log(`  ✅ Updated version in: ${file} (from ${currentVersion} to ${version})`);
            } else {
                console.log(`  ℹ️ No matching version string found in: ${file}`);
            }
        } else {
            console.log(`  ⚠️ File not found: ${file}`);
        }
    });

	console.log(`🏁 Complete`);
}

updateFilesVersion();
