# Event Flow Diagram

```mermaid
flowchart TD
    ApplicationStart --> LoginRequest
    LoginRequest --> LoginSuccess
    LoginSuccess --> MainMenuReady
```