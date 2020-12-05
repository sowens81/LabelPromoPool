import React, { Component } from "react";
import ArtistDataService from "../services/artist.service";

export default class Artist extends Component {
  constructor(props) {
    super(props);
    this.onChangeName = this.onChangeName.bind(this);
    this.onChangeProfilePictureURL= this.onChangeProfilePictureURL.bind(this);
    this.onChangeBeatportUrl= this.onChangeBeatportUrl.bind(this);
    this.onChangeSoundCloudUrl= this.onChangeSoundCloudUrl.bind(this);    
    this.getArtist = this.getArtist.bind(this);
    this.updatePublished = this.updatePublished.bind(this);
    this.updateArtist = this.updateArtist.bind(this);
    this.deleteArtist = this.deleteArtist.bind(this);

    this.state = {
      currentArtist: {
        id: null,
        name: "",
        profilePictureURL: "",
        beatportUrl: "",
        soundCloudUrl: "",
        published: false
      },
      message: ""
    };
  }

  componentDidMount() {
    this.getArtist(this.props.match.params.id);
  }

  onChangeName(e) {
    const name = e.target.value;

    this.setState(function(prevState) {
      return {
        currentArtist: {
          ...prevState.currentArtist,
          name: name
        }
      };
    });
  }

  onChangeProfilePictureURL(e) {
    const profilePictureURL = e.target.value;
    
    this.setState(prevState => ({
      currentArtist: {
        ...prevState.currentArtist,
        profilePictureURL: profilePictureURL
      }
    }));
  }

  onChangeBeatportUrl(e) {
    const beatportUrl = e.target.value;
    
    this.setState(prevState => ({
      currentArtist: {
        ...prevState.currentArtist,
        beatportUrl: beatportUrl
      }
    }));
  }

  onChangeSoundCloudUrl(e) {
    const soundCloudUrl = e.target.value;
    
    this.setState(prevState => ({
      currentArtist: {
        ...prevState.currentArtist,
        soundCloudUrl: soundCloudUrl
      }
    }));
  }

  getArtist(id) {
    ArtistDataService.get(id)
      .then(response => {
        this.setState({
          currentArtist: response.data
        });
        console.log(response.data);
      })
      .catch(e => {
        console.log(e);
      });
  }

  updatePublished(status) {
    var data = {
      id: this.state.currentArtist.id,
      name: this.state.currentArtist.name,
      profilePictureURL: this.state.currentArtist.profilePictureURL,
      beatportUrl: this.state.currentArtist.beatportUrl,
      soundCloudUrl: this.state.currentArtist.soundCloudUrl,
      published: status
    };

    ArtistDataService.update(this.state.currentArtist.id, data)
      .then(response => {
        this.setState(prevState => ({
          currentArtist: {
            ...prevState.currentArtist,
            published: status
          }
        }));
        console.log(response.data);
      })
      .catch(e => {
        console.log(e);
      });
  }

  updateArtist() {
    ArtistDataService.update(
      this.state.currentArtist.id,
      this.state.currentArtist
    )
      .then(response => {
        console.log(response.data);
        this.setState({
          message: "The Artist was updated successfully!"
        });
      })
      .catch(e => {
        console.log(e);
      });
  }

  deleteArtist() {    
    ArtistDataService.delete(this.state.currentArtist.id)
      .then(response => {
        console.log(response.data);
        this.props.history.push('/artists')
      })
      .catch(e => {
        console.log(e);
      });
  }

  render() {
    const { currentArtist } = this.state;

    return (
      <div>
        {currentArtist ? (
          <div className="edit-form">
            <h4>Artist</h4>
            <form>
              <div className="form-group">
                <label htmlFor="name">Name</label>
                <input
                  type="text"
                  className="form-control"
                  id="name"
                  value={currentArtist.name}
                  onChange={this.onChangeName}
                />
              </div>
              <div className="form-group">
                <label htmlFor="profilePictureURL">Profile Picture URL</label>
                <input
                  type="text"
                  className="form-control"
                  id="profilePictureURL"
                  value={currentArtist.profilePictureURL}
                  onChange={this.onChangeProfilePictureURL}
                />
              </div>

              <div className="form-group">
                <label htmlFor="beatportUrl">Beatport URL</label>
                <input
                  type="text"
                  className="form-control"
                  id="beatportUrl"
                  value={currentArtist.beatportUrl}
                  onChange={this.onChangeBeatportUrl}
                />
              </div>

              <div className="form-group">
                <label htmlFor="soundCloudUrl">SoundCloud URL</label>
                <input
                  type="text"
                  className="form-control"
                  id="soundCloudUrl"
                  value={currentArtist.soundCloudUrl}
                  onChange={this.onChangeSoundCloudUrl}
                />
              </div>

              <div className="form-group">
                <label>
                  <strong>Status:</strong>
                </label>
                {currentArtist.published ? "Published" : "Pending"}
              </div>
            </form>

            {currentArtist.published ? (
              <button
                className="badge badge-primary mr-2"
                onClick={() => this.updateArtist(false)}
              >
                UnPublish
              </button>
            ) : (
              <button
                className="badge badge-primary mr-2"
                onClick={() => this.updatePublished(true)}
              >
                Publish
              </button>
            )}

            <button
              className="badge badge-danger mr-2"
              onClick={this.deleteArtist}
            >
              Delete
            </button>

            <button
              type="submit"
              className="badge badge-success"
              onClick={this.updateArtist}
            >
              Update
            </button>
            <p>{this.state.message}</p>
          </div>
        ) : (
          <div>
            <br />
            <p>Please click on an Artist...</p>
          </div>
        )}
      </div>
    );
  }
}