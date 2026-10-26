export class WorkerRequestState {
  #generation = 0;
  #readyGeneration = null;
  #latestRequest = null;
  #pendingRequest = null;

  get generation() {
    return this.#generation;
  }

  get ready() {
    return this.#readyGeneration === this.#generation;
  }

  replaceWorker() {
    this.#generation++;
    this.#readyGeneration = null;
    return this.#generation;
  }

  markReady(generation) {
    if (generation !== this.#generation) return false;
    this.#readyGeneration = generation;
    return true;
  }

  markFailed(generation) {
    if (generation !== this.#generation) return false;
    this.#readyGeneration = null;
    return true;
  }

  enqueue(request) {
    this.#latestRequest = request;
    this.#pendingRequest = request;
  }

  shouldDrain(running) {
    return this.ready && !running && this.#pendingRequest !== null;
  }

  takePending() {
    if (!this.ready || !this.#pendingRequest) return null;
    const request = { ...this.#pendingRequest, workerGeneration: this.#generation };
    this.#latestRequest = request;
    this.#pendingRequest = null;
    return request;
  }

  isCurrent(request, workspaceRevision, profile) {
    return (
      this.#latestRequest !== null &&
      request.workspaceRevision === workspaceRevision &&
      request.profile === profile &&
      request.protocolVersion === this.#latestRequest.protocolVersion &&
      request.workerGeneration === this.#generation &&
      request.requestId === this.#latestRequest.requestId &&
      request.workspaceRevision === this.#latestRequest.workspaceRevision &&
      request.profile === this.#latestRequest.profile
    );
  }

  isCurrentWorkspaceRequest(request, workspaceRevision, profile) {
    return (
      this.#latestRequest !== null &&
      request.protocolVersion === this.#latestRequest.protocolVersion &&
      request.requestId === this.#latestRequest.requestId &&
      request.workspaceRevision === workspaceRevision &&
      request.workspaceRevision === this.#latestRequest.workspaceRevision &&
      request.profile === profile &&
      request.profile === this.#latestRequest.profile
    );
  }

  matchesResponse(request, response, workspaceRevision, profile) {
    return (
      this.isCurrent(request, workspaceRevision, profile) &&
      response.protocolVersion === request.protocolVersion &&
      response.workerGeneration === request.workerGeneration &&
      response.requestId === request.requestId &&
      response.workspaceRevision === request.workspaceRevision &&
      response.profile === request.profile
    );
  }
}
