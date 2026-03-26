#!/usr/bin/env bash
# Creates a self-contained macOS .app bundle for CnE2PLC.
#
# Usage:
#   ./scripts/publish-macos.sh              # builds osx-arm64 (default)
#   ./scripts/publish-macos.sh osx-x64      # builds osx-x64 (Intel)
#   ./scripts/publish-macos.sh osx-arm64 osx-x64   # builds both

set -euo pipefail

PROJ="CnE2PLC.Avalonia/CnE2PLC.Avalonia.csproj"
APP_NAME="CnE2PLC"
RIDS=("${@:-osx-arm64}")

build_bundle() {
    local RID="$1"
    local PUBLISH_DIR="artifacts/${RID}/publish"
    local BUNDLE="${APP_NAME}.app"
    local BUNDLE_DIR="artifacts/${RID}/${BUNDLE}"
    local ZIP="artifacts/${RID}/${APP_NAME}-${RID}.zip"

    echo ""
    echo "=== Publishing for ${RID} ==="
    dotnet publish "${PROJ}" \
        -c Release \
        -r "${RID}" \
        --self-contained true \
        -o "${PUBLISH_DIR}" \
        --nologo

    echo "--- Creating .app bundle ---"
    rm -rf "${BUNDLE_DIR}"
    mkdir -p "${BUNDLE_DIR}/Contents/MacOS"
    mkdir -p "${BUNDLE_DIR}/Contents/Resources"

    # Copy all published output into Contents/MacOS/
    cp -r "${PUBLISH_DIR}/"* "${BUNDLE_DIR}/Contents/MacOS/"

    # Place Info.plist at Contents/
    cp "CnE2PLC.Avalonia/Info.plist" "${BUNDLE_DIR}/Contents/Info.plist"

    # Ensure the executable is marked executable
    chmod +x "${BUNDLE_DIR}/Contents/MacOS/CnE2PLC.Avalonia"

    echo "--- Zipping ${BUNDLE} ---"
    rm -f "${ZIP}"
    (cd "artifacts/${RID}" && zip -qr "$(basename "${ZIP}")" "${BUNDLE}")

    echo "Built: ${ZIP}"
}

for RID in "${RIDS[@]}"; do
    build_bundle "${RID}"
done

echo ""
echo "Done."
