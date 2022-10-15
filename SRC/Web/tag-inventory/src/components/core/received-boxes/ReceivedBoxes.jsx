import React, { useEffect, useRef } from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import Pagination from "../../common/reuseable/Pagination";
import config from "../../../config.json";
import Table from "../../common/ui-controls/Table";
import Actions from "../../common/reuseable/Actions";
import ImportScanActions from "../../common/reuseable/ImportScanActions";
import { getReceivedBoxes } from "../../../actions/receivedBoxActions";
import {
  GET_SHIPMENTS,
  GET_SHIPMENT,
  GET_RECEIVED_BOXES,
  GET_RECEIVED_BOX
} from "../../../actions/actionTypes";
import { Link } from "react-router-dom";

const ReceivedBoxes = props => {
  const pageNumber = useRef(1);
  const filterSetting = useRef({});
  const pageSize = config.pageConfig.pageSize;
  const shipmentID =
    props.match && props.match.params.shipmentId
      ? props.match.params.shipmentId
      : -1;

  if (shipmentID !== -1) {
    filterSetting.current["shipmentID"] = shipmentID;
  }

  const columns = [
    {
      path: "receivedBoxID",
      title: "ReceivedBoxID",
      key: 1,
      content: id => {
        const navigateUrl = `/received-box/summary/${id}`;
        return <Link to={navigateUrl}>{id}</Link>;
      }
    },
    { path: "startTag", title: "StartFrom", key: 2 },
    { path: "endTag", title: "StartTo", key: 3 },
    { path: "quantity", title: "Quantity", key: 4 },
    { path: "status", title: "Status", key: 5 }
  ];

  const getURL = () => {
    let baseUrl = `${process.env.REACT_APP_API_URL}received-box/list?pageSize=${pageSize}&pageNumber=${pageNumber.current}`;
    let props = Object.keys(filterSetting.current);

    if (props.length > 0) {
      for (let prop in filterSetting.current) {
        baseUrl = baseUrl + `&${prop}=${filterSetting.current[prop]}`;
      }
    }
    console.log(baseUrl);
    return baseUrl;
  };

  useEffect(() => {
    const url = getURL();
    props.getReceivedBoxes(url);
  }, []);

  const handleChange = ({ value, element, column }) => {
    switch (element) {
      case "pagination":
        pageNumber.current = parseInt(value);
        break;
      case "dropdown":
        if (parseInt(value) === -1) delete filterSetting.current["statusID"];
        else filterSetting.current["statusID"] = value;
        pageNumber.current = 1;
        break;
      case "tablefilter":
        if (value) filterSetting.current[column] = value;
        else delete filterSetting.current[column];
        pageNumber.current = 1;
        break;
    }
    const url = getURL();
    props.getReceivedBoxes(url);
  };

  const handleClick = arg => {
    switch (arg.target.action) {
      case "back":
        props.history.goBack();
        break;
    }
  };

  return (
    <React.Fragment>
      <div className="actions">
        <ImportScanActions
          actionFormType="importBox"
          navigateURL="/received-box/import"
          onLinkClick={handleClick}
        ></ImportScanActions>
      </div>

      <div className="content-area-main">
        <div className="row top-buffer">
          <div className="col-3 select-container">
            <select
              className="select-search"
              onChange={e =>
                handleChange({
                  value: e.target.value,
                  element: "dropdown"
                })
              }
            >
              <option className="select-search" selected value={-1}>
                All Received Boxes
              </option>
              <option className="select-search" value={0}>
                Imported
              </option>
              <option className="select-search" value={1}>
                Delivery Verified OK
              </option>
              <option className="select-search" value={2}>
                Tags Not In Delivery
              </option>
              <option className="select-search" value={3}>
                Additional Tags In Delivery
              </option>
            </select>
          </div>
        </div>
        <div className="row top-buffer">
          <div className="list-container">
            {
              <Table
                columns={columns}
                data={props.receivedBoxes}
                searchVisible={true}
                onTableFilter={handleChange}
              ></Table>
            }
          </div>
          <div className="row">
            <Pagination
              totalRecords={props.searchCount}
              pageSize={pageSize}
              onPageChange={handleChange}
              selectedPage={pageNumber.current}
            ></Pagination>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

ReceivedBoxes.propTypes = {
  receivedBoxes: PropTypes.array.isRequired,
  shipment: PropTypes.object.isRequired,
  totalCount: PropTypes.number.isRequired,
  searchCount: PropTypes.number.isRequired,
  getReceivedBoxes: PropTypes.func.isRequired
};

const mapStateToProps = state => ({
  receivedBoxes: state.receivedBox.receivedBoxes,
  shipment: state.receivedBox.shipment,
  totalCount: state.receivedBox.totalCount,
  searchCount: state.receivedBox.searchCount
});

export default connect(
  mapStateToProps,
  { getReceivedBoxes }
)(ReceivedBoxes);
