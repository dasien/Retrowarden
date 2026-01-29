#!/bin/bash
################################################################################
# on-session-end-cost.sh - Extract cost from session transcript
#
# Thin passthrough hook that delegates cost extraction to Python.
# Receives transcript path from Claude CLI and passes it to cmat.
################################################################################

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(cd "$SCRIPT_DIR/../.." && pwd)"

# Debug log
DEBUG_LOG="$PROJECT_ROOT/.claude/logs/hook_debug.log"
mkdir -p "$(dirname "$DEBUG_LOG")" 2>/dev/null || true
echo "[$(date -u +%Y-%m-%dT%H:%M:%SZ)] Hook started" >> "$DEBUG_LOG"

# Read hook input from stdin
HOOK_INPUT=$(cat)
echo "[$(date -u +%Y-%m-%dT%H:%M:%SZ)] Hook input: $HOOK_INPUT" >> "$DEBUG_LOG"

# Check if we're in a task execution context
TASK_ID="${CMAT_CURRENT_TASK_ID:-}"
echo "[$(date -u +%Y-%m-%dT%H:%M:%SZ)] TASK_ID=$TASK_ID" >> "$DEBUG_LOG"

if [ -z "$TASK_ID" ]; then
    echo "[$(date -u +%Y-%m-%dT%H:%M:%SZ)] No TASK_ID, exiting" >> "$DEBUG_LOG"
    exit 0
fi

# Extract session info from hook input
TRANSCRIPT_PATH=$(echo "$HOOK_INPUT" | jq -r '.transcript_path // ""')
SESSION_ID=$(echo "$HOOK_INPUT" | jq -r '.session_id // ""')

echo "[$(date -u +%Y-%m-%dT%H:%M:%SZ)] TRANSCRIPT_PATH=$TRANSCRIPT_PATH" >> "$DEBUG_LOG"

if [ -z "$TRANSCRIPT_PATH" ] || [ ! -f "$TRANSCRIPT_PATH" ]; then
    echo "[$(date -u +%Y-%m-%dT%H:%M:%SZ)] Transcript not found, exiting" >> "$DEBUG_LOG"
    exit 0
fi

# Get CMAT root from global settings file
SETTINGS_FILE="$HOME/.claude_queue_ui/settings.json"
CMAT_ROOT=""
if [ -f "$SETTINGS_FILE" ]; then
    CMAT_ROOT=$(jq -r '.cmat_root // ""' "$SETTINGS_FILE")
fi

echo "[$(date -u +%Y-%m-%dT%H:%M:%SZ)] CMAT_ROOT=$CMAT_ROOT" >> "$DEBUG_LOG"

if [ -z "$CMAT_ROOT" ] || [ ! -d "$CMAT_ROOT/src/core" ]; then
    echo "[$(date -u +%Y-%m-%dT%H:%M:%SZ)] CMAT not found, exiting" >> "$DEBUG_LOG"
    exit 0
fi

# Set PYTHONPATH and run cost extraction
export PYTHONPATH="$CMAT_ROOT/src:${PYTHONPATH:-}"

# Use CMAT's venv first (has required dependencies like pyyaml),
# then fall back to project venv or system python
if [ -f "$CMAT_ROOT/.venv/bin/python" ]; then
    PYTHON_CMD="$CMAT_ROOT/.venv/bin/python"
elif [ -f "$PROJECT_ROOT/.venv/bin/python" ]; then
    PYTHON_CMD="$PROJECT_ROOT/.venv/bin/python"
else
    PYTHON_CMD="python3"
fi

echo "[$(date -u +%Y-%m-%dT%H:%M:%SZ)] Using Python: $PYTHON_CMD" >> "$DEBUG_LOG"
echo "[$(date -u +%Y-%m-%dT%H:%M:%SZ)] Running cost extraction" >> "$DEBUG_LOG"
RESULT=$("$PYTHON_CMD" -m core costs extract "$TASK_ID" "$TRANSCRIPT_PATH" "$SESSION_ID" 2>&1) || true
echo "[$(date -u +%Y-%m-%dT%H:%M:%SZ)] Result: $RESULT" >> "$DEBUG_LOG"