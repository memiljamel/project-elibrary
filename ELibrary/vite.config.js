﻿import { defineConfig } from 'vite';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';

const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

fs.mkdirSync(baseFolder, { recursive: true });

const certificateName = "elibrary";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    if (0 !== child_process.spawnSync('dotnet', [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
    ], { stdio: 'inherit', }).status) {
        throw new Error("Could not create certificate.");
    }
}

// https://vitejs.dev/config/
export default defineConfig({
    appType: 'custom',
    root: 'Assets',
    publicDir: 'public',
    build: {
        emptyOutDir: false,
        manifest: true,
        outDir: '../wwwroot',
        assetsDir: 'build',
        rollupOptions: {
            input: [
                'Assets/css/site.css',
                'Assets/js/site.js',
            ],
        },
    },
    server: {
        strictPort: true,
        https: {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
        },
    },
    optimizeDeps: {
        include: [],
    },
});
