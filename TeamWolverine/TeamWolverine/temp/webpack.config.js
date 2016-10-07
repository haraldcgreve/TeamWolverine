var path = require('path');

module.exports = {
  entry: './assets/_js/main.js',
  output: {
    path: './',
    filename: './dist/index.js'
  },
  devServer: {
    inline: true,
    port: 3333
  },
  module: {
    loaders: [
      {
        test: /\.js$/,
        exclude: /node_modules/,
        loader: 'babel',
        query: {
          presets: ['es2015']
        }
      }
    ]
  }
}
