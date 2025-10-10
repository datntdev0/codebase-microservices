import gulp from "gulp";
import _ from "lodash";

import { gulpConfig } from "../gulp.config.js";
import { objectWalkRecursive, outputFunc, bundle } from "./helpers.js";
import { cleanTask } from "./clean.js";

// merge with default parameters
const args = Object.assign(
    {
        prod: false,
        sourcemap: false,
        exclude: "",
        path: "",
        suffix: false,
    }
);

const tasks = [];

if (args.prod === true) {
    // force disable debug for production
    gulpConfig.config.debug = false;
    // force assets minification for production
    gulpConfig.config.compile.jsMinify = true;
    gulpConfig.config.compile.cssMinify = true;
}

// task to bundle js/css
let buildBundleTask = (cb) => {
    var streams = [];
    objectWalkRecursive(gulpConfig.build, function (val, key) {
        if (val.hasOwnProperty("src") && val.hasOwnProperty("dist")) {
            if (["custom", "media", "api", "misc"].indexOf(key) !== -1) {
                outputFunc(val);
            } else {
                streams = bundle(val);
            }
        }
    });
    cb();
    return streams;
};

tasks.push(cleanTask);
tasks.push(buildBundleTask);

// entry point
const compileTask = gulp.series(...tasks);

// Exports
export { compileTask };
