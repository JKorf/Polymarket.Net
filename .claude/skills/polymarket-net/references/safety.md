# Polymarket.Net operational guidance

Read this for credential, trading and private-stream workflows. Generating an
example does not execute its account operations; keep any execution within the
user's requested scope.

## Credentials and identifiers

- Use placeholders, environment variables or injected secret configuration; do not embed or log real private keys, API secrets or passphrases.
- Prediction L1 wallet signing uses `EthKey`; prediction L2 uses key/secret/passphrase. An order needs wallet signing and L2 authentication. User streams authenticate with L2.
- Perps uses its own owner/proxy/secret model. Owner keys register proxies; proxy private keys sign trades; proxy address plus secret authenticate private reads and streams. Prediction L2 credentials do not authenticate Perps.
- For prediction V1 use `ClobTokenIds`; for V2 use `PositionIds` and `marketVersion: 2`. Condition ids, Gamma numeric ids and token/position ids are distinct.
- Concrete Perps methods use discovered numeric instrument ids. Check instrument precision, minimum notional and applicable market status before trading.
- Prediction prices are between 0 and 1. Perps uses instrument-specific prices and `PerpTimeInForce`.

## Trading and results

- Make live order, cancellation, proxy registration, API-key, transfer and withdrawal operations clear in examples. Use the account operations needed for the user's workflow.
- For sample limit-order lifecycles, show cancellation of the created order when appropriate. Cancellation may fail if an order has already filled; inspect its result.
- REST and socket results require outer `.Success` checks before `.Data`. Prediction orders additionally require `.Data.Success`; Perps single-order rejections are outer failures. Check each batch order result too. Prediction cancellations additionally expose `Canceled` and `NotCanceled`; inspect individual refusals even when the outer call succeeds.
- Retry transient reads with bounded backoff when appropriate. For uncertain order responses, reconcile order state before retrying to avoid duplicate orders.
- Prediction allowance methods distinguish collateral, conditional and V2 conditional assets. Do not assume an allowance refresh substitutes for any external approval/funding requirement.
- `PostOrderHeartbeatAsync` belongs to services intentionally managing heartbeat-protected prediction orders.

## Nonces, proxy changes and uncertain submissions

Proxy registration/deletion, API-key creation, leverage/margin, transfer and withdrawal calls change account state. Register the intended proxy with the owner key and retain its returned secret. Do not use an unregistered proxy key or substitute Prediction L2 credentials. Respect expiration and revoke only the proxy intended by the application.

A limit order can execute immediately. Cancellation can race with fills, so a successful cancellation is not proof of no exposure. Do not add another exchange's hedge-mode or contract-count assumptions to Perps; inspect current positions and the supported close/reduce-only parameters.

`Error.IsTransient` alone does not justify repeating signed mutations. A timeout or connection loss may leave an order or account change accepted but unconfirmed. Retain order/client/transaction identifiers and reconcile orders, trades, balances, proxy/account state or transfer history before deciding to resubmit. Allow for propagation delays and partial batches. Bounded backoff is appropriate for transient public reads.

## Builder settings

Prediction and Perps builder settings belong to their own API/transport options. Perps builder fee defaults to `0.01m` (0.01%, one basis point). Builder approval is an account change; configure the requested builder and fee policy explicitly. Null builder code disables that API's builder attribution; do not silently change it while implementing unrelated behavior.
