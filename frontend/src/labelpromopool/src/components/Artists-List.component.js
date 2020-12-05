import React, { Component } from "react";
import ArtistDataService from "../services/artist.service";
import { Link } from "react-router-dom";

export default class ArtistList extends Component {
    constructor(props) {
      super(props);
      this.onChangeSearchName = this.onChangeSearchName.bind(this);
      this.retrieveArtists = this.retrieveArtists.bind(this);
      this.refreshList = this.refreshList.bind(this);
      this.setActiveArtist = this.setActiveArtist.bind(this);
      this.removeAllArtists = this.removeAllArtists.bind(this);
      this.searchName = this.searchName.bind(this);
  
      this.state = {
        artists: [],
        currentArtist: null,
        currentIndex: -1,
        searchName: ""
      };
    }

    componentDidMount() {
        this.retrieveArtists();
    }

    onChangeSearchName(e) {
        const searchName = e.target.value;
    
        this.setState({
          searchName: searchName
        });
      }

      retrieveArtists() {
        ArtistDataService.getAll()
          .then(response => {
            this.setState({
              artists: response.data
            });
            console.log(response.data);
          })
          .catch(e => {
            console.log(e);
          });
      }

      refreshList() {
        this.retrieveArtists();
        this.setState({
          currentArtist: null,
          currentIndex: -1
        });
      }
      
      setActiveArtist(artist, index) {
        this.setState({
          currentArtist: artist,
          currentIndex: index
        });
      }

      removeAllArtists() {
        ArtistDataService.deleteAll()
          .then(response => {
            console.log(response.data);
            this.refreshList();
          })
          .catch(e => {
            console.log(e);
          });
      }

      searchName() {
        ArtistDataService.findByName(this.state.searchName)
          .then(response => {
            this.setState({
              artists: response.data
            });
            console.log(response.data);
          })
          .catch(e => {
            console.log(e);
          });
      }

      render() {
        const { searchName, artists, currentArtist, currentIndex } = this.state;

        return (
            <div className="list row">
              <div className="col-md-8">
                <div className="input-group mb-3">
                  <input
                    type="text"
                    className="form-control"
                    placeholder="Search by name"
                    value={searchName}
                    onChange={this.onChangeSearchName}
                  />
                  <div className="input-group-append">
                    <button
                      className="btn btn-outline-secondary"
                      type="button"
                      onClick={this.searchName}
                    >
                      Search
                    </button>
                  </div>
                </div>
              </div>
              <div className="col-md-6">
                <h4>Artists List</h4>
      
                <ul className="list-group">
                  {artists &&
                    artists.map((artist, index) => (
                      <li
                        className={
                          "list-group-item " +
                          (index === currentIndex ? "active" : "")
                        }
                        onClick={() => this.setActiveArtist(artist, index)}
                        key={index}
                      >
                        {artist.name}
                      </li>
                    ))}
                </ul>
      
                <button
                  className="m-3 btn btn-sm btn-danger"
                  onClick={this.removeAllArtists}
                >
                  Remove All
                </button>
              </div>
              <div className="col-md-6">
                {currentArtist ? (
                  <div>
                    <h4>Artist</h4>
                    <div>
                      <label>
                        <strong>Name:</strong>
                      </label>{" "}
                      {currentArtist.name}
                    </div>
                    <div>
                      <label>
                        <strong>Profile Picture:</strong>
                      </label>{" "}
                      {currentArtist.profilePictureURL}
                    </div>
                    <div>
                      <label>
                        <strong>Beatport URL:</strong>
                      </label>{" "}
                      {currentArtist.beatportUrl}
                    </div>
                    <div>
                      <label>
                        <strong>SoundCloud URL:</strong>
                      </label>{" "}
                      {currentArtist.soundCloudUrl}
                    </div>
                    <div>
                      <label>
                        <strong>Status:</strong>
                      </label>{" "}
                      {currentArtist.published ? "Published" : "Pending"}
                    </div>
      
                    <Link
                      to={"/artists/" + currentArtist.id}
                      className="badge badge-warning"
                    >
                      Edit
                    </Link>
                  </div>
                ) : (
                  <div>
                    <br />
                    <p>Please click on an Artist...</p>
                  </div>
                )}
              </div>
            </div>
          );
      }
    }