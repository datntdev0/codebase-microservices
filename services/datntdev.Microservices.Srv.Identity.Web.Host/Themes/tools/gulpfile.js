import { cleanTask } from "./gulp/clean.js";
import { watchTask } from "./gulp/watch.js";
import { compileTask } from "./gulp/compile.js";

// Clean tasks:
export { cleanTask as clean };

// Watch tasks:
export { watchTask as watch };

// Main tasks:
export { compileTask as compile };

