import sha1 from "js-sha1";

export function convertPlainTextToSHA1(source) {
   return btoa(String.fromCharCode(...new Uint8Array(sha1.array(source))));
}

