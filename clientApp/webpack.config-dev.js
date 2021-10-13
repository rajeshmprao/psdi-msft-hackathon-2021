/* eslint-disable @typescript-eslint/no-var-requires */
/* eslint-disable no-undef */
const path = require("path");
const ESLintPlugin = require("eslint-webpack-plugin");
const config = {
  mode: "development",
  watch: true,
  entry: {
    index: "./src/index.tsx",
  },
  output: {
    path: path.join(__dirname, "../wwwroot/js"),
    filename: "bundle.js",
  },
  resolve: {
    extensions: [".ts", ".tsx", ".js", ".jsx"],
    fallback: { crypto: false, buffer: false, fs: false },
  },
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        use: "babel-loader",
        exclude: /node_modules/,
      },
      {
        test: /\.css$/i,
        use: ["style-loader", "css-loader"],
      },
      {
        test: /\.svg$/,
        use: {
          loader: "svg-url-loader",
          options: {
            encoding: "base64",
          },
        },
      },
    ],
  },
  plugins: [
    new ESLintPlugin({
      extensions: ["ts", "tsx"],
      exclude: ["node_modules", "wwwroot", "*.css", "*.svg"],
      files: ["./ClientApp/**/*/"],
    }),
  ],
};

module.exports = config;
