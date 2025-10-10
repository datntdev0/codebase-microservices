const gulpConfig = {
	name: "{theme}",
	desc: "Gulp build config",
	version: "{version}",
	config: {
		debug: false,
		compile: {
			jsMinify: false,
			cssMinify: false,
			jsSourcemaps: false,
			cssSourcemaps: false,
		},
		path: {
			src: "../src",
			node_modules: "node_modules",
		},
		dist: ["../../wwwroot/bundles"],
	},
	build: {
		base: {
			src: {
				styles: ["{$config.path.src}/sass/style.scss"],
				scripts: [
                    "{$config.path.node_modules}/sweetalert2/dist/sweetalert2.js",
					"{$config.path.src}/js/**/*.js",
				],
			},
			dist: {
				styles: "{$config.dist}/css/style.css",
				scripts: "{$config.dist}/js/scripts.js",
			},
		},
	},
};

export { gulpConfig };
