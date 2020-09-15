#solution.py
from diagrams import Diagram
from diagrams.azure.integration import APIManagement
from diagrams.azure.compute import FunctionApps
from diagrams.azure.database import CosmosDb
from diagrams.azure.network import TrafficManagerProfiles

graph_attr = {
    "fontsize": "2",
    "bgcolor": "transparent"
}

with Diagram("Label Promo Pool", show=False, direction="TB"):
    tfm = TrafficManagerProfiles("Traffic Manager")
    apim1 = APIManagement("API-Management-UKsouth")
    apim2 = APIManagement("API-Management-UKwest")
    db = CosmosDb("db")
    worker1 = FunctionApps("Label-API")
    worker2 = FunctionApps("Label-API")
    worker3 = FunctionApps("Label-API")
    tfm >> apim1
    tfm >> apim2
    apim1 >> worker1
    apim1 >> worker2
    apim1 >> worker3
    apim2 >> worker1
    apim2 >> worker2
    apim2 >> worker3
    worker1 >> db
    worker2 >> db
    worker3 >> db
        



