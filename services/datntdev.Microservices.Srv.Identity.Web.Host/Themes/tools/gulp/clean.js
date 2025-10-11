import { deleteAsync } from 'del';
import { gulpConfig } from "../gulp.config.js";

// task to clean and delete dist directory content
const getPaths = () => {
    const paths = ['!config', ...gulpConfig.config.dist];
    const realpaths = paths.map(x => x + "/*");
    return realpaths;
};

export const cleanTask = () => {
    return deleteAsync(getPaths(), { force: true });
};
