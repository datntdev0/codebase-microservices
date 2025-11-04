import { defineConfig } from "cypress";

export default defineConfig({
  projectId: "datntdev.Microservices.Angular",
  video: true,
  e2e: {
    viewportHeight: 720,
    viewportWidth: 1280,
    specPattern: "cypress/e2e/**/*.spec.{js,jsx,ts,tsx}",
    testIsolation: false,
  },
});
