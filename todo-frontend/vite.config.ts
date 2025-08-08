import {defineConfig, loadEnv} from 'vite'
import react from '@vitejs/plugin-react-swc'

// https://vite.dev/config/
export default defineConfig(({mode}) => {
    const env = loadEnv(mode, process.cwd(), '')

    return {
        plugins: [react()],
        server: {
            port: parseInt(env.VITE_PORT),
            proxy: {
                // "apiservice" is the name of the API in AppHost.cs.
                '/api': {
                    target: process.env.services__apiservice__https__0 || process.env.services__apiservice__http__0,
                    changeOrigin: true,
                    secure: false,
                    rewrite: (path) => path.replace(/^\/api/, '/api')
                }
            }
        },
        build:{
            outDir: 'dist',
            rollupOptions: {
                input: './index.html'
            }
        }
    }
})