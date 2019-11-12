export function timeout(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
export async function sleep(fn, duration, ...args) {
    await timeout(3000);
    return fn(...args);
}
