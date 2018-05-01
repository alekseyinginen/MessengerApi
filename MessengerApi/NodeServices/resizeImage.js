const sharp = require('sharp');

module.exports = function (result, path, maxWidth) {
    sharp(path)
        .resize(maxWidth)
        .pipe(result.stream);
}