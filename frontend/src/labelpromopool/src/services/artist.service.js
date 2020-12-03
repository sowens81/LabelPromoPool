import http from "../http-common";

class TutorialDataService {
  getAll() {
    return http.get("/artist");
  }

  get(id) {
    return http.get(`/artist/${id}`);
  }

  create(data) {
    return http.post("/artist", data);
  }

  update(id, data) {
    return http.put(`/artist/${id}`, data);
  }

  delete(id) {
    return http.delete(`/arist/${id}`);
  }

  deleteAll() {
    return http.delete(`/artist`);
  }

  findByTitle(title) {
    return http.get(`/artist?artistname=${name}`);
  }
}

export default new ArtistDataService();