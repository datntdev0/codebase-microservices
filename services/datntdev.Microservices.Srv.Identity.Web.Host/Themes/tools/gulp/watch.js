import gulp from "gulp";
import { compileTask } from "./compile.js";

const watchTask = () => {
  return gulp.watch(
    [build.config.path.src + "/**/*.js", build.config.path.src + "/**/*.scss"],
    gulp.series(compileTask)
  );
};

// Exports
export { watchTask };
