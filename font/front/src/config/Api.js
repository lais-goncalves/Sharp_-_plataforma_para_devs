const _Api = () => {
    const urlDevWatch = process.env.REACT_APP_API_PORT_WATCH.toString();
    const urlDevStill = process.env.REACT_APP_API_PORT_STILL.toString();
    const urls = {
        DEV: `http://localhost:${urlDevStill}`,
        PRD: `http://localhost:`
    };

    const urlAtual = process.env.NODE_ENV == 'development' ? urls.DEV : urls.PRD;

    // TODO: quebrar método em partes menores
    const fetchBack = async (urlApi, queryApi, metadata) => {
        try {
            let query = new URLSearchParams(queryApi).toString();

            let _urlApi = urlApi;
            if (!_urlApi || _urlApi[0] !== "/") {
                _urlApi = "/" + _urlApi;
            }

            const urlCompleta = urlAtual + urlApi + (query ? `?${query}` : '');

            if (!metadata) {
                metadata = {};
            }
            metadata.credentials = "include";

            const resposta = await fetch(urlCompleta, metadata);
            if (!resposta.ok) {
                console.log(`Response status: ${resposta.status}`);
            }

            const resultado = resposta.json();
            return resultado;
        } 
        
        catch (error) {
            console.log(error.message);
        }
    }

    return ({
        urls: urls,
        fetch: fetchBack
    });
}

const Api = _Api();
export default Api;