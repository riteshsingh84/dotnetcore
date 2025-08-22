# Copilot Chat Custom Modes

This folder contains four custom Copilot Chat modes for Visual Studio Code:

## Key Fields Explained

   **name:**	Display name of the mode in VSCode.

   **description:**	Short summary of what the mode does.

   **mode:**	Usually "chat" for conversational modes.

   **tools:**	Can be read-only, edit, or full depending on what the assistant can do.
   
   **Instructions:**	Detailed behavior and personality of the assistant.

## Modes Included

1. **Security Audit Assistant**
   - Reviews code for vulnerabilities and suggests secure practices.

   *Ask questions like:*

            “Audit this file for security issues.”
            “Are there any risks in how user input is handled?”
            “Is this authentication flow secure?”    

2. **Bug Triage Assistant**
   - Helps analyze, prioritize, and suggest fixes for bugs.

   *Ask questions like:*

            “Analyze this stack trace and suggest a fix.”
            “What’s the likely cause of this crash?”
            “How should we prioritize this bug?”    

3. **Performance Optimization Assistant**
   - Identifies performance bottlenecks and recommends optimizations.

   *Ask questions like:*
      
         “Can you find performance issues in this function?”
         “How can I optimize this loop for large datasets?”
         “Is this database query efficient?”


4. **Review Assistant**
   - Assists in code review by analyzing changes and suggesting improvements.

      
## How to Use

1. Place the `.copilot-modes` folder in your VSCode workspace.
2. Open the Copilot Chat panel.
3. Select the desired mode from the dropdown.
4. Start interacting with the assistant based on your workflow.

## Best Practices

- Use the appropriate mode for the task at hand.
- Keep instructions focused and clear.
- Share this folder with your team via version control.

