const { src, dest,watch, series } = require('gulp');
const sass = require('gulp-sass')(require('sass'));

function sassCompile() {
    return src('wwwroot/main.scss')
        .pipe(sass())
        .pipe(dest('wwwroot/css'))
}

function sassWatch() {
    watch(['wwwroot/main.scss'], sassCompile)
}

exports.default = series(sassCompile,sassWatch)