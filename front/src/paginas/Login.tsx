import React, { useState } from "react";

function Login() {
	const [usuario, setUsuario] = useState("");
	const [senha, setSenha] = useState("");

	function logar() {
		if (!!usuario && !!senha) {
			console.log("ok");
		}
	}

	return (
		<>
			<input
				type="text"
				placeholder="usuário"
				value={usuario}
				onChange={(e) => setUsuario(e.target.value)}
			/>
			<br />
			<input
				type="password"
				placeholder="senha"
				value={senha}
				onChange={(e) => setSenha(e.target.value)}
			/>
			<br />
			<input type="submit" value="Login" onClick={logar} />
		</>
	);
}

export default Login;
