import jwt_decode from "jwt-decode";
import crypto from 'crypto-js';
import encBase64 from "crypto-js/enc-base64";
import formatHex from "crypto-js/format-hex";
import encBase64url from "crypto-js/enc-base64url";

// Check JWT Token
const verifyToken = () => {

    const token = sessionStorage.getItem('authorization');

    if(token == null){
        localStorage.removeItem('authorization');
        window.location.assign(window.location.origin);
        window.location = '/';
    }

    // Token exists, decode and check credentials
    const decoded = jwt_decode(token);
    const decodedHeader = jwt_decode(token, {header: true});
    const parts = token.split(".")
    const now = new Date().getTime();

    // Check correct Alg
    // Check correct type
    // Check valid Issuer
    // Check valid Signature
    // Check if Not Before Time has passed yet
    // Check if Expired 
    // Check if time is before Issued At
    const signature = encBase64url.stringify(crypto.HmacSHA256(parts[0] + '.' + parts[1], 'XMwiwDaL8LZQwDJPHkYEJytE9C1wPxcOTi1Ks2IA'));
    console.log(signature)
    console.log(decodedHeader.alg)
    console.log(decodedHeader.typ)
    console.log(decoded.iss)
    console.log(decoded.nbf * 1000)
    console.log(decoded.exp * 1000)
    console.log(decoded.iat * 1000)
    console.log(now)

    if(decodedHeader.alg !== "HS256" || decodedHeader.typ !== "JWT" || decoded.iss !== "TrialByFire.Tresearch" || 
        parts[2] !== signature || decoded.nbf * 1000 > now || now > decoded.exp * 1000 || now < decoded.iat * 1000)
    {
        localStorage.removeItem('authorization');
        window.location.assign(window.location.origin);
        window.location = '/';
    }
}

export default verifyToken;